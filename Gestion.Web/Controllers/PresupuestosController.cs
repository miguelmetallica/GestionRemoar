using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,Presupuestos")]
    
    public class PresupuestosController : Controller
    {
        private readonly IPresupuestosRepository repository;
        private readonly IProductosRepository productosRepository;
        private readonly IClientesRepository clientesRepository;
        private readonly ITiposDocumentosRepository tiposDocumentosRepository;
        private readonly IProvinciasRepository provinciasRepository;
        private readonly IPresupuestosDescuentosRepository descuentosRepository;
        private readonly ITiposResponsablesRepository tiposResponsablesRepository;
        private readonly IComprobantesRepository comprobantesRepository;
        private readonly IUserHelper userHelper;
        private readonly IConfiguracionesRepository configuraciones;

        public PresupuestosController(IPresupuestosRepository repository,
            IProductosRepository productosRepository, 
            IClientesRepository clientesRepository,
            ITiposDocumentosRepository tiposDocumentosRepository,
            IProvinciasRepository provinciasRepository,
            IPresupuestosDescuentosRepository descuentosRepository,
            ITiposResponsablesRepository tiposResponsablesRepository,
            IComprobantesRepository comprobantesRepository,
            IUserHelper userHelper,
            IConfiguracionesRepository configuraciones
            )
        {
            this.repository = repository;
            this.productosRepository = productosRepository;
            this.clientesRepository = clientesRepository;
            this.tiposDocumentosRepository = tiposDocumentosRepository;
            this.provinciasRepository = provinciasRepository;
            this.descuentosRepository = descuentosRepository;
            this.tiposResponsablesRepository = tiposResponsablesRepository;
            this.comprobantesRepository = comprobantesRepository;
            this.userHelper = userHelper;
            this.configuraciones = configuraciones;
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

                if (presup == null)
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

                        if (presup == null)
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

            if (presupuesto == null) 
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            var detalle = await this.repository.spPresupuestosDetallePresupuesto(id);
            ViewData["Detalle"] = detalle;
            return View(presupuesto);
        }

        public async Task<IActionResult> TiposResponsables(string id)
        {
            var presupuesto = await this.repository.spPresupuestosPendiente(id);
            ViewBag.TiposResponsables = this.tiposResponsablesRepository.GetCombo();
            return View(presupuesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TiposResponsables(PresupuestosDTO presupuestosDTO)
        {
            if (presupuestosDTO == null)
            {
                return NotFound();
            }
            
            var presup = await this.repository.spPresupuestosPendiente(presupuestosDTO.Id);

            if (presup == null)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            if (presup.TipoResponsableId == null)
            {
                await this.repository.spTipoResponsableAplica(presupuestosDTO.Id, presupuestosDTO.TipoResponsableId, User.Identity.Name);
            }
            else 
            { 
                if (presup.TipoResponsableId != presupuestosDTO.TipoResponsableId) 
                {
                    await this.repository.spTipoResponsableAplica(presupuestosDTO.Id, presupuestosDTO.TipoResponsableId, User.Identity.Name);
                }
            }


            return RedirectToAction("Pendiente", new { id = presupuestosDTO.Id });
        }

        public async Task<IActionResult> Descuento(string id)
        {
            var presupuesto = await this.repository.spPresupuestosPendiente(id);

            if (presupuesto == null)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }
            var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewData["Descuentos"] = descuentosRepository.GetAll()
                    .Where(x => x.Estado == true && x.UsuarioId == user.Id)
                    .OrderBy(x => x.Porcentaje);
            return View(presupuesto);
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

            if (presup == null)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            await this.repository.spDescuentoAplica(id2, id,User.Identity.Name);
            return RedirectToAction("Pendiente", new { id = id2 });
        }

        public async Task<IActionResult> DescuentoBorrar(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var presup = await this.repository.spPresupuestosPendiente(id);
            if (presup == null)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            await this.repository.spDescuentoBorrar(id,User.Identity.Name);
            return RedirectToAction("Pendiente", new { id });
        }

        public async Task<IActionResult> DatosFiscalesBorrar(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presup = await this.comprobantesRepository.spComprobante(id);
            if (presup == null)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            await this.repository.spDatosFiscalesBorrar(id, User.Identity.Name);
            return RedirectToAction("ComprobanteDetalle", new { id });
        }
        public async Task<IActionResult> ProductoCreate(string id)
        {
            ViewData["productos"] =await productosRepository.spProductosVentaGet();

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
                var prod_comodin = configuraciones.GetAll().Where(x => x.Configuracion == "PRODUCTO_COMODIN").FirstOrDefault();

                if (prod_comodin == null) 
                {
                    return RedirectToAction("Pendientes", "Presupuestos");
                }

                var producto = await productosRepository.spProductosOneCodigo(prod_comodin.Valor);
                if (producto == null)
                {
                    return RedirectToAction("Pendientes", "Presupuestos");
                }

                presupuestos.ProductoId = producto.Id;
                presupuestos.ProductoCodigo = producto.Codigo;
                

                await repository.AddItemComodinAsync(presupuestos, User.Identity.Name);
                return RedirectToAction("Pendiente", new { id = presupuestos.Id });
            }
            
            return View(presupuestos);
        }
        public async Task<IActionResult> ProductoDelete(string id)
        {

            if (id == null)
            {
                return NotFound();
            }
            
            return View(await this.repository.spPresupuestosDetalleId(id));
        }

        [HttpPost, ActionName("ProductoDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var presupuestos = await repository.spPresupuestosDetalleId(id);

            var presup = await this.repository.spPresupuestosPendiente(presupuestos.PresupuestoId);
            if (presup == null)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            await repository.DeleteDetailAsync(presupuestos.Id);
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

            if (presup == null)
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

            if (presup == null)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            var prod = presup;//.Where(x => x.DetalleId == id).FirstOrDefault();
            if (prod.CantidadProductos != 1)
            {
                await this.repository.ModifyCantidadesAsync(id, -1);
            }
            else
            {
                await this.repository.DeleteDetailAsync(id);
            }
            
            return RedirectToAction("Pendiente", new { id = id2 });
        }

        public async Task<IActionResult> ProductoSeleccion(PresupuestosDetalle presupuestos)
        {
            var presup = await this.repository.spPresupuestosPendiente(presupuestos.PresupuestoId);

            if (presup == null)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            var prod = await productosRepository.spProductosVentaGetOne(presupuestos.ProductoId);            
            presupuestos.Cantidad = 1;
            presupuestos.Precio = (decimal)prod.PrecioVenta;
            presupuestos.PrecioContado = (decimal)prod.PrecioContado;
            presupuestos.ProductoCodigo = prod.Codigo;
            presupuestos.ProductoNombre = prod.Producto;
            await repository.AddItemAsync(presupuestos, User.Identity.Name);
            
            return RedirectToAction("Pendiente", new { id = presupuestos.PresupuestoId });
        }

        public async Task<IActionResult> PresupuestoPendienteAprobar(string id)
        {
            var presupuesto = await this.repository.spPresupuestosPendiente(id);

            if (presupuesto == null)
            {
                return RedirectToAction("AprobarRechazar", "Presupuestos");
            }

            if (presupuesto.CantidadProductos == 0)
            {
                return RedirectToAction("PresupuestoAprobarRechazar", new { id = id });
            }

            presupuesto.UsuarioAprobacionRechazo = User.Identity.Name;
            await repository.spAprobar(presupuesto);

            return RedirectToAction("AprobarRechazar");
        }

        public async Task<IActionResult> PresupuestoPendienteRechazar(string id)
        {
            var presupuesto = await this.repository.spPresupuestosPendiente(id);

            if (presupuesto == null)
            {
                return RedirectToAction("AprobarRechazar", "Presupuestos");
            }            

            presupuesto.UsuarioAprobacionRechazo = User.Identity.Name;
            await repository.spRechazar(presupuesto);

            return RedirectToAction("AprobarRechazar");
        }

        public async Task<IActionResult> Vencidos()
        {
            var model = await repository.spPresupuestosVencidos();
            return View(model);
        }

        public async Task<IActionResult> Vencido(string id)
        {
            var presupuesto = await this.repository.spPresupuestosVencido(id);

            if (presupuesto == null)
            {
                return RedirectToAction("Vencido", "Presupuestos");
            }
            ViewData["Detalle"] = await this.repository.spPresupuestosDetallePresupuesto(id);
            return View(presupuesto);
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
            ViewData["Detalle"] = await this.repository.spPresupuestosDetallePresupuesto(id); ;
            return View(presupuesto);
        }

        public async Task<IActionResult> Aprobados()
        {
            var model = await comprobantesRepository.spComprobantesPresupuestos();
            return View(model);
        }

        //public async Task<IActionResult> Aprobado(string id)
        //{
        //    var presupuesto = await this.repository.spPresupuestosAprobado(id);

        //    if (presupuesto == null)
        //    {
        //        return RedirectToAction("Aprobados", "Presupuestos");
        //    }
        //    ViewData["Detalle"] = presupuesto;
        //    return View(presupuesto.FirstOrDefault());
        //}

        public async Task<IActionResult> AprobarRechazar()
        {
            var model = await repository.spPresupuestosPendientes();
            return View(model);
        }

        public async Task<IActionResult> PresupuestoAprobarRechazar(string id)
        {
            var presupuesto = await this.repository.spPresupuestosPendiente(id);

            if (presupuesto == null)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }
            ViewData["Detalle"] = await this.repository.spPresupuestosDetallePresupuesto(id);
            return View(presupuesto);
        }

        public async Task<IActionResult> PresupuestoImprimir(string id)
        {
            var presupuesto = await this.repository.spPresupuestosImprimir(id);
            if (presupuesto == null)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            var cliente = await this.clientesRepository.spCliente(presupuesto.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Cliente"] = cliente;
            ViewData["Detalle"] = await this.repository.spPresupuestosDetallePresupuesto(id);
            return View(presupuesto);
        }

        public async Task<IActionResult> ComprobanteDetalle(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comprobante = await comprobantesRepository.spComprobante(id);
            if (comprobante == null)
            {
                return NotFound();
            }

            var cliente = await this.clientesRepository.spCliente(comprobante.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Comprobante"] = comprobante;
            ViewData["Comprobantes"] = await comprobantesRepository.spComprobantesPresupuesto(comprobante.PresupuestoId);
            ViewData["Detalles"] = await comprobantesRepository.spComprobanteDetallePresupuestoGet(comprobante.PresupuestoId);
            ViewData["FormasPagos"] = await comprobantesRepository.spComprobantesFormasPagosPresupuesto(comprobante.PresupuestoId);
            //ViewData["Remitos"] = await comprobantesRepository.spComprobanteDetallePresupuestoGet(comprobante.PresupuestoId);

            return View(cliente);
        }

        public async Task<IActionResult> PresupuestoCopia(string id)
        {
            var presupuesto = await this.repository.spPresupuesto(id);
            presupuesto.UsuarioAlta = User.Identity.Name;
            var nuevoId = Guid.NewGuid().ToString();
            await repository.spPresupuestoCopiar(presupuesto, nuevoId);

            return RedirectToAction("Pendiente", "Presupuestos", new { id = nuevoId });
        }

    }
}
