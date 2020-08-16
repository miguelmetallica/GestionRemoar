using Gestion.Web.Data;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public PresupuestosController(IPresupuestosRepository repository,
            IProductosRepository productosRepository, 
            IClientesRepository clientesRepository,
            ITiposDocumentosRepository tiposDocumentosRepository,
            IProvinciasRepository provinciasRepository
            )
        {
            this.repository = repository;
            this.productosRepository = productosRepository;
            this.clientesRepository = clientesRepository;
            this.tiposDocumentosRepository = tiposDocumentosRepository;
            this.provinciasRepository = provinciasRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await repository.GetOrdersAsync(this.User.Identity.Name);
            return View(model);            
        }

        public IActionResult ClienteBuscar(string id)
        {
            ViewBag.Id = id;
            return this.View(this.clientesRepository.GetAllActivos());
        }

        public IActionResult ClienteSeleccion(Presupuestos presupuestos)
        {
            var presupuesto = this.repository.GetPresupuestoId(presupuestos.Id);
            presupuestos.UsuarioAlta = User.Identity.Name;

            if (presupuesto == null)
            {
                repository.spInsertar(presupuestos);
            }
            else
            {
                presupuestos.EstadoId = presupuesto.EstadoId;
                repository.spEditar(presupuestos);
            }
            
            return RedirectToAction("Presupuesto", new { id = presupuestos.Id });
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
                var cuitcuil = await this.clientesRepository.ExistCuitCuilAsync("", clientes.CuilCuit);
                if (cuitcuil)
                {
                    ModelState.AddModelError("CuilCuit", "El Cuil ya esta cargado en la base de datos");
                }

                if (ModelState.IsValid)
                {
                    clientes.Id = Guid.NewGuid().ToString();
                    clientes.UsuarioAlta = User.Identity.Name;
                    var result = await clientesRepository.spInsertar(clientes);
                    Presupuestos presupuesto = this.repository.GetPresupuestoId(clientes.ExternalId);                    

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
                        presupuesto.ClienteId = clientes.Id;
                        presupuesto.UsuarioAlta = User.Identity.Name;
                        presupuesto.EstadoId = presupuesto.EstadoId;
                        await repository.spEditar(presupuesto);
                    }

                    return RedirectToAction("Presupuesto", new { id = presupuesto.Id });
                }
            }

            ViewBag.TiposDocumentos = this.tiposDocumentosRepository.GetCombo();
            ViewBag.Provincias = this.provinciasRepository.GetCombo();

            return View(clientes);
        }

        public IActionResult Presupuesto(string id)
        {
            var presupuesto = this.repository.GetPresupuestoId(id);
            return View(presupuesto);
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
                producto.PrecioNormal = presupuestos.Precio;
                producto.UsuarioAlta = User.Identity.Name;
                producto.EsVendedor = true;
                var result = await productosRepository.spInsertar(producto);
                presupuestos.ProductoId = producto.Id;

                await repository.AddItemAsync(presupuestos, User.Identity.Name);
                return RedirectToAction("Presupuesto", new { id = presupuestos.Id });
            }
            
            return View(presupuestos);
        }

        //public IActionResult AddProduct()
        //{
        //    var model = new AddItemViewModel
        //    {
        //        Cantidad = 1,
        //        Productos = this.productosRepository.GetComboProducts()
        //    };

        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddProduct(PresupuestosDetalle model)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        await this.repository.AddItemAsync(model, this.User.Identity.Name);
        //        return this.RedirectToAction("Create");
        //    }

        //    return this.View(model);
        //}

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
            var presupuestos = repository.GetDetalle(id);
            await repository.DeleteDetailAsync(id);
            return RedirectToAction("Presupuesto", new { id = presupuestos.PresupuestoId });
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
            

            await this.repository.ModifyCantidadesAsync(id, 1);
            return RedirectToAction("Presupuesto", new { id = id2 });
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


            await this.repository.ModifyCantidadesAsync(id, -1);
            return RedirectToAction("Presupuesto", new { id = id2 });
        }

        public async Task<IActionResult> ProductoSeleccion(PresupuestosDetalle presupuestos)
        {
            var prod = await productosRepository.GetByIdAsync(presupuestos.ProductoId);
            presupuestos.Cantidad = 1;
            presupuestos.Precio = (decimal)prod.PrecioNormal;
            await repository.AddItemAsync(presupuestos, User.Identity.Name);
            
            return RedirectToAction("Presupuesto", new { id = presupuestos.PresupuestoId });
        }

        public async Task<IActionResult> PresupuestoPendienteAprobar(string id)
        {
            var presupuestos = await repository.GetByIdAsync(id);
            
            presupuestos.EstadoId = "aeb79e2d-c583-44f2-bbd2-d60b63dd6c6c";
            await repository.UpdateAsync(presupuestos);

            return RedirectToAction("Presupuesto", new { id = id });
        }
    }
}
