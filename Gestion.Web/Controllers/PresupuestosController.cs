using Gestion.Web.Data;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize]
    public class PresupuestosController : Controller
    {
        private readonly IPresupuestosRepository repository;
        private readonly IProductosRepository productosRepository;
        private readonly IClientesRepository clientesRepository;
        private readonly ITiposDocumentosRepository tiposDocumentosRepository;
        private readonly IProvinciasRepository provinciasRepository;
        private readonly IPresupuestosDescuentosRepository descuentosRepository;

        public PresupuestosController(IPresupuestosRepository repository,
            IProductosRepository productosRepository, 
            IClientesRepository clientesRepository,
            ITiposDocumentosRepository tiposDocumentosRepository,
            IProvinciasRepository provinciasRepository,
            IPresupuestosDescuentosRepository descuentosRepository
            )
        {
            this.repository = repository;
            this.productosRepository = productosRepository;
            this.clientesRepository = clientesRepository;
            this.tiposDocumentosRepository = tiposDocumentosRepository;
            this.provinciasRepository = provinciasRepository;
            this.descuentosRepository = descuentosRepository;
        }

        public async Task<IActionResult> Pendientes()
        {
            var model = await repository.spPresupuestosPendientes();                
            return View(model);            
        }

        public IActionResult ClienteBuscar(string id)
        {
            ViewBag.Id = id;
            return this.View(this.clientesRepository.GetAllActivos());
        }

        public async Task<IActionResult> ClienteSeleccion(Presupuestos presupuestos)
        {
            var presupuesto = this.repository.GetPresupuestoPendientesId(presupuestos.Id);
            presupuestos.UsuarioAlta = User.Identity.Name;

            if (presupuesto == null)
            {
                await repository.spInsertar(presupuestos);
            }
            else
            {
                var presup = await this.repository.spPresupuestosPendiente(presupuestos.Id);

                if (presup == null || presup.Count == 0)
                {
                    return RedirectToAction("Pendientes", "Presupuestos");
                }

                await repository.spEditar(presupuestos);
            }
            
            return RedirectToAction("Pendiente", new { id = presupuestos.Id });
        }

        public IActionResult ClienteCreate(string id)
        {            
            ViewBag.TiposDocumentos = this.tiposDocumentosRepository.GetCombo();
            ViewBag.Provincias = this.provinciasRepository.GetCombo();
            var cliente = new ClientesFisicoAdd();
            cliente.ExternalId = id;
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClienteCreate(ClientesFisicoAdd clientes)
        {
            if (ModelState.IsValid)
            {
                var doc = await this.clientesRepository.ExistNroDocAsync("", clientes.TipoDocumentoId, clientes.NroDocumento);
                if (doc)
                {
                    ModelState.AddModelError("NroDocumento", "El Tipo y Nro de Documento ya esta cargado en la base de datos");
                }
                if (clientes.CuilCuit != null)                 
                {
                    var cuitcuil = await this.clientesRepository.ExistCuitCuilAsync("", clientes.CuilCuit);
                    if (cuitcuil)
                    {
                        ModelState.AddModelError("CuilCuit", "El Cuil ya esta cargado en la base de datos");
                    }
                }
                
                if (ModelState.IsValid)
                {
                    clientes.Id = Guid.NewGuid().ToString();
                    clientes.UsuarioAlta = User.Identity.Name;
                    var result = await clientesRepository.spInsertar(clientes);
                    Presupuestos presupuesto = this.repository.GetPresupuestoPendientesId(clientes.ExternalId);                    

                    if (presupuesto == null)
                    {
                        presupuesto = new Presupuestos();                        
                        presupuesto.Id = clientes.ExternalId;
                        presupuesto.ClienteId = clientes.Id;
                        presupuesto.UsuarioAlta = User.Identity.Name;
                        await repository.spInsertar(presupuesto);
                    }
                    else
                    {
                        var presup = await this.repository.spPresupuestosPendiente(presupuesto.Id);

                        if (presup == null || presup.Count == 0)
                        {
                            return RedirectToAction("Pendientes", "Presupuestos");
                        }

                        presupuesto.ClienteId = clientes.Id;
                        presupuesto.UsuarioAlta = User.Identity.Name;
                        await repository.spEditar(presupuesto);
                    }

                    return RedirectToAction("Pendiente", new { id = presupuesto.Id });
                }
            }

            ViewBag.TiposDocumentos = this.tiposDocumentosRepository.GetCombo();
            ViewBag.Provincias = this.provinciasRepository.GetCombo();

            return View(clientes);
        }

        public async Task<IActionResult> Pendiente(string id)
        {
            var presupuesto = await this.repository.spPresupuestosPendiente(id);

            if (presupuesto == null || presupuesto.Count == 0) 
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }            
            ViewData["Detalle"] = presupuesto;
            return View(presupuesto.FirstOrDefault());
        }

        public async Task<IActionResult> Descuento(string id)
        {
            var presupuesto = await this.repository.spPresupuestosPendiente(id);

            if (presupuesto == null || presupuesto.Count == 0)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }
            ViewData["Descuentos"] = descuentosRepository.GetAll().Where(x => x.Estado == true).OrderBy(x => x.Porcentaje);
            return View(presupuesto.FirstOrDefault());
        }

        public async Task<IActionResult> DescuentoAplica(string id, string id2)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id2 == null)
            {
                return NotFound();
            }

            var presup = await this.repository.spPresupuestosPendiente(id2);

            if (presup == null || presup.Count == 0)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            await this.repository.spDescuentoAplica(id2, id,User.Identity.Name);
            return RedirectToAction("Pendiente", new { id = id2 });
        }

        public IActionResult ProductoCreate(string id)
        {
            ViewData["productos"] = productosRepository.GetAll();

            var presupuestodetalle  = new PresupuestosDetalle();
            presupuestodetalle.PresupuestoId = id;
            return View(presupuestodetalle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductoCreate(PresupuestosDetalle presupuestos)
        {
            if (ModelState.IsValid)
            {
                var producto = new Productos();
                producto.Id = Guid.NewGuid().ToString();
                producto.Producto = presupuestos.NombreProducto;
                producto.PrecioVenta = presupuestos.Precio;
                producto.UsuarioAlta = User.Identity.Name;
                producto.EsVendedor = true;
                var result = await productosRepository.spInsertar(producto);
                presupuestos.ProductoId = producto.Id;

                var presup = await this.repository.spPresupuestosPendiente(presupuestos.Id);

                if (presup == null || presup.Count == 0)
                {
                    return RedirectToAction("Pendientes", "Presupuestos");
                }

                await repository.AddItemAsync(presupuestos, User.Identity.Name);
                return RedirectToAction("Pendiente", new { id = presupuestos.Id });
            }
            
            return View(presupuestos);
        }
        public IActionResult ProductoDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(this.repository.GetDetalle(id));
        }

        [HttpPost, ActionName("ProductoDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var presupuestos = await repository.GetDetalle(id);

            var presup = await this.repository.spPresupuestosPendiente(presupuestos.PresupuestoId);

            if (presup == null || presup.Count == 0)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            await repository.DeleteDetailAsync(id);
            return RedirectToAction("Pendiente", new { id = presupuestos.PresupuestoId });
        }        

        public async Task<IActionResult> Incrementar(string id,string id2)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id2 == null)
            {
                return NotFound();
            }

            var presup = await this.repository.spPresupuestosPendiente(id2);

            if (presup == null || presup.Count == 0)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            await this.repository.ModifyCantidadesAsync(id, 1);
            return RedirectToAction("Pendiente", new { id = id2 });
        }

        public async Task<IActionResult> Decrementar(string id, string id2)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id2 == null)
            {
                return NotFound();
            }

            var presup = await this.repository.spPresupuestosPendiente(id2);

            if (presup == null || presup.Count == 0)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            await this.repository.ModifyCantidadesAsync(id, -1);
            return RedirectToAction("Pendiente", new { id = id2 });
        }

        public async Task<IActionResult> ProductoSeleccion(PresupuestosDetalle presupuestos)
        {
            var presup = await this.repository.spPresupuestosPendiente(presupuestos.PresupuestoId);

            if (presup == null || presup.Count == 0)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            var prod = await productosRepository.GetByIdAsync(presupuestos.ProductoId);
            presupuestos.Cantidad = 1;
            presupuestos.Precio = (decimal)prod.PrecioVenta;
            await repository.AddItemAsync(presupuestos, User.Identity.Name);
            
            return RedirectToAction("Pendiente", new { id = presupuestos.PresupuestoId });
        }

        public async Task<IActionResult> PresupuestoPendienteAprobar(string id)
        {
            var presup = await this.repository.spPresupuestosPendiente(id);

            if (presup == null || presup.Count == 0)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            if (presup[0].Cantidad == 0)
            {
                return RedirectToAction("Pendiente", new { id = id });
            }

            var presupuesto = await this.repository.GetByIdAsync(id);
            presupuesto.UsuarioAlta = User.Identity.Name;
            await repository.spAprobar(presupuesto);

            return RedirectToAction("Pendientes");
        }

        public async Task<IActionResult> PresupuestoPendienteRechazar(string id)
        {
            var presup = await this.repository.spPresupuestosPendiente(id);

            if (presup == null || presup.Count == 0)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }            

            var presupuesto = await this.repository.GetByIdAsync(id);
            presupuesto.UsuarioAlta = User.Identity.Name;
            await repository.spRechazar(presupuesto);

            return RedirectToAction("Pendientes");
        }

        public async Task<IActionResult> Vencidos()
        {
            var model = await repository.spPresupuestosVencidos();
            return View(model);
        }

        public async Task<IActionResult> Vencido(string id)
        {
            var presupuesto = await this.repository.spPresupuestosVencido(id);

            if (presupuesto == null || presupuesto.Count == 0)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }
            ViewData["Detalle"] = presupuesto;
            return View(presupuesto.FirstOrDefault());
        }

        public async Task<IActionResult> PresupuestoVencidoCopia(string id)
        {
            var presupuesto = await this.repository.GetByIdAsync(id);
            presupuesto.UsuarioAlta = User.Identity.Name;
            var nuevoId = Guid.NewGuid().ToString();
            await repository.spVencidoCopiar(presupuesto, nuevoId);

            return RedirectToAction("Pendiente","Presupuestos",new { id = nuevoId });
        }

        public async Task<IActionResult> Rechazados()
        {
            var model = await repository.spPresupuestosRechazados();
            return View(model);
        }

        public async Task<IActionResult> Rechazado(string id)
        {
            var presupuesto = await this.repository.spPresupuestosRechazado(id);

            if (presupuesto == null)
            {
                return RedirectToAction("Vencidos", "Presupuestos");
            }
            ViewData["Detalle"] = presupuesto;
            return View(presupuesto.FirstOrDefault());
        }

        public async Task<IActionResult> Aprobados()
        {
            var model = await repository.spPresupuestosAprobados();
            return View(model);
        }

        public async Task<IActionResult> Aprobado(string id)
        {
            var presupuesto = await this.repository.spPresupuestosAprobado(id);

            if (presupuesto == null)
            {
                return RedirectToAction("Aprobados", "Presupuestos");
            }
            ViewData["Detalle"] = presupuesto;
            return View(presupuesto.FirstOrDefault());
        }

    }
}
