using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,Comprobantes")]
    
    public class ComprobantesController : Controller
    {
        private readonly IComprobantesRepository repository; 
        private readonly IUserHelper userHelper;
        private readonly IClientesRepository clientesRepository;

        public ComprobantesController(IComprobantesRepository repository, 
                                        IUserHelper userHelper,
                                        IClientesRepository clientesRepository)
        {
            this.repository = repository;
            this.userHelper = userHelper;
            this.clientesRepository = clientesRepository;
        }

        public async Task<IActionResult> ImputacionesProductos()
        {
            var model = await repository.spComprobantesImputaciones();
            return View(model);
        }

        public async Task<IActionResult> ImputacionProducto(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var CC = await repository.spComprobante(id);
            if (CC == null)
            {
                return NotFound();
            }

            var cliente = await this.clientesRepository.spCliente(CC.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }
            
            ViewData["Detalle"] = await repository.spComprobanteDetalleImputacion(CC.Id);
            ViewData["Clientes"] = cliente;

            return View(CC);
        }

        [HttpPost]
        public JsonResult ImputaProducto(ComprobantesDetalleDTO detalleDTO)
        {
            try
            {
                detalleDTO.UsuarioAlta = User.Identity.Name;
                var i = repository.spComprobanteDetalleInsertImputacion(detalleDTO);
                return Json(1);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return Json(0);
            }
        }


        [HttpPost]
        public JsonResult EntregaProducto(ComprobantesDetalleDTO detalleDTO)
        {
            try
            {
                detalleDTO.UsuarioAlta = User.Identity.Name;
                var i = repository.spComprobanteDetalleInsertEntrega(detalleDTO);
                return Json(1);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult EntregaAnulaProducto(ComprobantesDetalleDTO detalleDTO)
        {
            try
            {
                detalleDTO.UsuarioAlta = User.Identity.Name;
                var i = repository.spComprobanteDetalleInsertEntregaAnula(detalleDTO);
                return Json(1);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult AutorizaProducto(ComprobantesDetalleDTO detalleDTO)
        {
            try
            {
                detalleDTO.UsuarioAlta = User.Identity.Name;
                var i = repository.spComprobanteDetalleInsertAutoriza(detalleDTO);
                return Json(1);                
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult AutorizaAnulaProducto(ComprobantesDetalleDTO detalleDTO)
        {
            try
            {
                detalleDTO.UsuarioAlta = User.Identity.Name;
                var i = repository.spComprobanteDetalleInsertAutorizaAnula(detalleDTO);
                return Json(1);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return Json(0);
            }
        }

        public async Task<IActionResult> EntregaProductos()
        {
            //var model = await repository.spPresupuestosComprobantesEntrega();
            var model = await repository.spComprobantesImputaciones();
            
            return View(model);
        }

        public async Task<IActionResult> EntregarProducto(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var CC = await repository.spComprobante(id);
            if (CC == null)
            {
                return NotFound();
            }

            var cliente = await this.clientesRepository.spCliente(CC.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Detalle"] = await repository.spComprobanteDetalleEntrega(CC.Id);
            ViewData["DetalleTmp"] = await repository.spComprobanteDetalleTMP(CC.Id);
            ViewData["Clientes"] = cliente;
            ViewData["Comprobantes"] = await repository.spComprobantesInputaciones(id);
            ViewData["Detalles"] = await repository.spComprobanteRemitosDevolucionesGet(id);
            
            return View(CC);
        }

        public async Task<IActionResult> EntregarTMPProducto(string id, string comp)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            if (comp == null)
            {
                return NotFound();
            }

            var remito = new ComprobantesDetalleDTO();
            remito.Id = id;
            remito.UsuarioAlta = User.Identity.Name;
            await repository.spComprobanteDetalleInsertEntregaTMP(remito);

            return RedirectToAction(nameof(EntregarProducto), new { Id = comp });
        }

        public async Task<IActionResult> EliminarTMPProducto(string id, string comp)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (comp == null)
            {
                return NotFound();
            }

            var remito = new ComprobantesDetalleDTO();
            remito.Id = id;
            remito.UsuarioAlta = User.Identity.Name;
            await repository.spComprobanteDetalleEliminarEntregaTMP(remito);

            return RedirectToAction(nameof(EntregarProducto), new { Id = comp });
        }

        public async Task<IActionResult> GenerarRemito(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var remito = new ComprobantesDetalleDTO();
            remito.ComprobanteId = id;
            remito.UsuarioAlta = User.Identity.Name;
            await repository.spRemito(remito);

            return RedirectToAction(nameof(EntregarProducto), new { Id = id });
        }

        public async Task<IActionResult> GenerarDevolucion(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var remito = new ComprobantesDetalleDTO();
            remito.ComprobanteId = id;
            remito.UsuarioAlta = User.Identity.Name;
            await repository.spDevolucion(remito);

            return RedirectToAction(nameof(DevolucionProductos));
        }

        public async Task<IActionResult> RemitoImprimir(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comprobante = await repository.spComprobante(id);
            if (comprobante == null)
            {
                return NotFound();
            }

            ViewData["Remitos"] = await repository.spComprobanteDetalleImprimirGet(id);

            return View(comprobante);
        }

        [HttpPost]
        public JsonResult InsertaDatosFiscales(ComprobantesDTO comprobantesDTO)
        {
            try
            {
                comprobantesDTO.UsuarioAlta = User.Identity.Name;
                var i = repository.spComprobanteInsertaDatosFiscales(comprobantesDTO);
                return Json(1);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult InsertaCodigoAutorizacion(ComprobantesFormasPagosDTO pagosDTO)
        {
            try
            {
                pagosDTO.UsuarioAlta = User.Identity.Name;
                var i = repository.spComprobanteInsertaCodigoAutorizacion(pagosDTO);
                return Json(1);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return Json(0);
            }
        }

        public async Task<IActionResult> DevolucionProductos()
        {
            var model = await repository.spPresupuestosRemitos();
            return View(model);
        }

        public async Task<IActionResult> DevolucionProducto(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var CC = await repository.spComprobante(id);
            if (CC == null)
            {
                return NotFound();
            }

            var cliente = await this.clientesRepository.spCliente(CC.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Detalle"] = await repository.spComprobanteDetalleDevolucion(CC.Id);
            ViewData["DetalleTmp"] = await repository.spComprobanteDetalleTMP(CC.Id);
            ViewData["Clientes"] = cliente;

            ViewData["Comprobantes"] = await repository.spComprobantesInputaciones(id);
            ViewData["Detalles"] = await repository.spComprobanteRemitosDevolucionesGet(id);
            return View(CC);
        }

        public async Task<IActionResult> DevolucionTMPProducto(string id, string comp)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (comp == null)
            {
                return NotFound();
            }

            var remito = new ComprobantesDetalleDTO();
            remito.Id = id;
            remito.UsuarioAlta = User.Identity.Name;
            await repository.spComprobanteDetalleInsertDevolucionTMP(remito);

            return RedirectToAction(nameof(DevolucionProducto), new { Id = comp });
        }

        public async Task<IActionResult> EliminarDevolucionTMPProducto(string id, string comp)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (comp == null)
            {
                return NotFound();
            }

            var remito = new ComprobantesDetalleDTO();
            remito.Id = id;
            remito.UsuarioAlta = User.Identity.Name;
            await repository.spComprobanteDetalleEliminarEntregaTMP(remito);

            return RedirectToAction(nameof(DevolucionProducto), new { Id = comp });
        }
    }
}
