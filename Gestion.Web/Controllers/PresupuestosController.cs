using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly ITiposResponsablesRepository tiposResponsables;
        private readonly IClientesCategoriasRepository clientesCategorias;
        private readonly IFormasPagosRepository formasPagosRepository;
        private readonly IFormasPagosCuotasRepository cuotasRepository;
        private readonly ICombosRepository combos;
        private readonly IFormasPagosCotizacionRepository cotizacionRepository;

        public PresupuestosController(IPresupuestosRepository repository,
            IProductosRepository productosRepository,
            IClientesRepository clientesRepository,
            ITiposDocumentosRepository tiposDocumentosRepository,
            IProvinciasRepository provinciasRepository,
            IPresupuestosDescuentosRepository descuentosRepository,
            ITiposResponsablesRepository tiposResponsablesRepository,
            IComprobantesRepository comprobantesRepository,
            IUserHelper userHelper,
            IConfiguracionesRepository configuraciones,
            ITiposResponsablesRepository tiposResponsables,
            IClientesCategoriasRepository clientesCategorias,
            IFormasPagosRepository formasPagosRepository,
            IFormasPagosCuotasRepository cuotasRepository,
            ICombosRepository combos,
            IFormasPagosCotizacionRepository cotizacionRepository
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
            this.tiposResponsables = tiposResponsables;
            this.clientesCategorias = clientesCategorias;
            this.formasPagosRepository = formasPagosRepository;
            this.cuotasRepository = cuotasRepository;
            this.combos = combos;
            this.cotizacionRepository = cotizacionRepository;
        }

        public async Task<IActionResult> Pendientes()
        {
            if (User.IsInRole("Admin"))
            {
                var model = await repository.spPresupuestosPendientes();
                return View(model);
            }
            else
            {
                var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
                var model = await repository.spPresupuestosPendientes(user.SucursalId);
                return View(model);
            }
            
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
            ViewBag.TiposResponsables = this.tiposResponsables.GetCombo();
            ViewBag.Categorias = this.clientesCategorias.GetCombo();
            var cliente = new ClientesFisicoAdd();
            cliente.ExternalId = id;
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClienteCreate(ClientesFisicoAdd clientes)
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


            ViewBag.TiposDocumentos = this.tiposDocumentosRepository.GetCombo();
            ViewBag.Provincias = this.provinciasRepository.GetCombo();
            ViewBag.TiposResponsables = this.tiposResponsables.GetCombo();
            ViewBag.Categorias = this.clientesCategorias.GetCombo();

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
            var cliente = await this.clientesRepository.spCliente(presupuesto.ClienteId);
            ViewData["Detalle"] = detalle;

            if (cliente != null)
            {
                if (cliente.Celular != null || cliente.Celular != "")
                {
                    ViewBag.url = "https://wa.me/54" + cliente.Celular + "?text=" + Request.Host.Value + "/Presupuestos/PresupuestoImprimir/" + presupuesto.Id;
                }
                else
                {
                    ViewBag.url = "";
                }
            }
            else
            {
                ViewBag.url = "";
            }
            ViewData["FormasPagos"] = formasPagosRepository.GetAll().Where(x => x.Estado == true);
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            ViewData["Resumen"] = await repository.spResumenPresupuesto(id);
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

            await this.repository.spDescuentoAplica(id2, id, User.Identity.Name);
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

            await this.repository.spDescuentoBorrar(id, User.Identity.Name);
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
            ViewData["productos"] = await productosRepository.spProductosVentaGet();

            var presupuestodetalle = new PresupuestosDetalle();
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

            ViewData["productos"] = await productosRepository.spProductosVentaGet();
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

            await repository.spElimina(presupuestos.Id, User.Identity.Name);
            return RedirectToAction("Pendiente", new { id = presupuestos.PresupuestoId });
        }

        public async Task<IActionResult> Incrementar(string id, string id2)
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

            await this.repository.spIncrementa(id, 1, User.Identity.Name);
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

            var prod = presup;
            if (prod.CantidadProductos != 1)
            {
                await this.repository.spDecrementa(id, 1, User.Identity.Name);
            }
            else
            {
                await this.repository.spElimina(id, User.Identity.Name);
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
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            var detalle = await this.repository.spPresupuestosDetallePresupuesto(id);
            var cliente = await this.clientesRepository.spCliente(presupuesto.ClienteId);
            ViewData["Detalle"] = detalle;

            if (cliente != null)
            {
                if (cliente.Celular != null || cliente.Celular != "")
                {
                    ViewBag.url = "https://wa.me/54" + cliente.Celular + "?text=" + Request.Host.Value + "/Presupuestos/PresupuestoImprimir/" + presupuesto.Id;
                }
                else
                {
                    ViewBag.url = "";
                }
            }
            else
            {
                ViewBag.url = "";
            }
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            ViewData["Resumen"] = await repository.spResumenPresupuesto(id);
            return View(presupuesto);
        }

        public async Task<IActionResult> PresupuestoVencidoCopia(string id)
        {
            var presupuesto = await this.repository.GetByIdAsync(id);
            presupuesto.UsuarioAlta = User.Identity.Name;
            var nuevoId = Guid.NewGuid().ToString();
            await repository.spVencidoCopiar(presupuesto, nuevoId);

            return RedirectToAction("Pendiente", "Presupuestos", new { id = nuevoId });
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
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            var detalle = await this.repository.spPresupuestosDetallePresupuesto(id);
            var cliente = await this.clientesRepository.spCliente(presupuesto.ClienteId);
            ViewData["Detalle"] = detalle;

            if (cliente != null)
            {
                if (cliente.Celular != null || cliente.Celular != "")
                {
                    ViewBag.url = "https://wa.me/54" + cliente.Celular + "?text=" + Request.Host.Value + "/Presupuestos/PresupuestoImprimir/" + presupuesto.Id;
                }
                else
                {
                    ViewBag.url = "";
                }
            }
            else
            {
                ViewBag.url = "";
            }
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            ViewData["Resumen"] = await repository.spResumenPresupuesto(id);
            return View(presupuesto);
        }

        public async Task<IActionResult> Aprobados()
        {
            var model = await comprobantesRepository.spComprobantesPresupuestos();
            return View(model);
        }

        public async Task<IActionResult> AprobarRechazar()
        {
            if (User.IsInRole("Admin"))
            {
                var model = await repository.spPresupuestosPendientes();
                return View(model);
            }
            else
            {
                var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
                var model = await repository.spPresupuestosPendientes(user.SucursalId);
                return View(model);
            }
        }

        public async Task<IActionResult> PresupuestoAprobarRechazar(string id)
        {
            var presupuesto = await this.repository.spPresupuestosPendiente(id);

            if (presupuesto == null)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            var detalle = await this.repository.spPresupuestosDetallePresupuesto(id);
            var cliente = await this.clientesRepository.spCliente(presupuesto.ClienteId);
            ViewData["Detalle"] = detalle;

            if (cliente != null)
            {
                if (cliente.Celular != null || cliente.Celular != "")
                {
                    ViewBag.url = "https://wa.me/54" + cliente.Celular + "?text=" + Request.Host.Value + "/Presupuestos/PresupuestoImprimir/" + presupuesto.Id;
                }
                else
                {
                    ViewBag.url = "";
                }
            }
            else
            {
                ViewBag.url = "";
            }
            ViewData["FormasPagos"] = formasPagosRepository.GetAll().Where(x => x.Estado == true);
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            ViewData["Resumen"] = await repository.spResumenPresupuesto(id);
            return View(presupuesto);
        }

        [AllowAnonymous]
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
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            ViewData["Resumen"] = await repository.spResumenPresupuesto(id);

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

        public async Task<IActionResult> Cheque(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spPresupuestosPendiente(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            var resumen = await repository.spResumenPresupuesto(id);
            ViewData["Resumen"] = resumen;
            ViewBag.Bancos = new SelectList(cuotasRepository.GetBancos(), "Value", "Text");

            var descuento = cuotasRepository.GetCuotaUno(fp);
            var recibo = new FormaPagoChequeDTO();
            recibo.ClienteId = ventas.ClienteId;
            recibo.VentaRapidaId = ventas.Id;
            recibo.FormaPagoId = fp;

            recibo.Descuento = 0;
            recibo.Recargo = 0;
            if (descuento.Interes > 0)
            {
                recibo.Recargo = Math.Abs(descuento.Interes);
                recibo.SaldoConDescuento = resumen.SaldoAPagar * (1 + Math.Abs(descuento.Interes / 100));
            }
            if (descuento.Interes < 0)
            {
                recibo.Descuento = Math.Abs(descuento.Interes);
                recibo.SaldoConDescuento = resumen.SaldoAPagar * (1 - Math.Abs(descuento.Interes / 100));
            }

            recibo.Saldo = resumen.SaldoAPagar;


            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cheque(FormaPagoChequeDTO formaPago)
        {
            var ventas = await this.repository.spPresupuestosPendiente(formaPago.VentaRapidaId);
            if (ventas == null)
            {
                return NotFound();
            }

            if (formaPago.Descuento == 0 && formaPago.Recargo == 0)
            {
                if (formaPago.Saldo < formaPago.Importe)
                {
                    ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo");
                }
            }
            else
            {
                if (formaPago.SaldoConDescuento < formaPago.Importe)
                {
                    ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo");
                }
            }

            if (ModelState.IsValid)
            {
                formaPago.Usuario = User.Identity.Name;
                await repository.spCheque(formaPago);

                return RedirectToAction(nameof(Pendiente), new { id = formaPago.VentaRapidaId });
            }


            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenPresupuesto(formaPago.VentaRapidaId);
            ViewBag.Bancos = new SelectList(cuotasRepository.GetBancos(), "Value", "Text");
            return View(formaPago);
        }

        public async Task<IActionResult> Efectivo(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spPresupuestosPendiente(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            var resumen = await repository.spResumenPresupuesto(id);
            ViewData["Resumen"] = resumen;

            var descuento = cuotasRepository.GetCuotaUno(fp);
            var recibo = new FormaPagoEfectivoDTO();
            recibo.ClienteId = ventas.ClienteId;
            recibo.VentaRapidaId = ventas.Id;
            recibo.FormaPagoId = fp;
            recibo.Descuento = Math.Abs(descuento.Interes);
            recibo.Saldo = resumen.SaldoAPagar;
            recibo.SaldoConDescuento = resumen.SaldoAPagar - (resumen.SaldoAPagar * (Math.Abs(descuento.Interes / 100)));
            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Efectivo(FormaPagoEfectivoDTO formaPago)
        {
            var ventas = await this.repository.spPresupuestosPendiente(formaPago.VentaRapidaId);
            if (ventas == null)
            {
                return NotFound();
            }

            if (formaPago.Descuento == 0)//&& formaPago.Recargo == 0)
            {
                if (formaPago.Saldo < formaPago.Importe)
                {
                    ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo");
                }
            }
            else
            {
                if (formaPago.SaldoConDescuento < formaPago.Importe)
                {
                    ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo");
                }
            }

            if (ModelState.IsValid)
            {
                formaPago.Usuario = User.Identity.Name;
                await repository.spEfectivo(formaPago);

                return RedirectToAction(nameof(Pendiente), new { id = formaPago.VentaRapidaId });
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenPresupuesto(formaPago.VentaRapidaId);
            return View(formaPago);
        }

        public async Task<IActionResult> TarjetaDebito(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spPresupuestosPendiente(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);

            var resumen = await repository.spResumenPresupuesto(id);
            ViewData["Resumen"] = resumen;

            ViewBag.Años = new SelectList(combos.GetAños().OrderBy(x => x.Text), "Value", "Text");
            ViewBag.Meses = new SelectList(combos.GetMeses().OrderBy(x => x.Text), "Value", "Text");
            ViewBag.Tarjetas = new SelectList(cuotasRepository.GetEntidades(fp), "Value", "Text");

            var recibo = new FormaPagoTarjetaDTO();
            recibo.ClienteId = ventas.ClienteId;
            recibo.VentaRapidaId = ventas.Id;
            recibo.FormaPagoId = fp;

            recibo.Descuento = 0;
            recibo.Recargo = 0;

            recibo.Saldo = resumen.SaldoAPagar;
            recibo.Cuota = 1;
            recibo.Interes = 0;
            recibo.Total = 0;

            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TarjetaDebito(FormaPagoTarjetaDTO formaPago)
        {
            formaPago.Cuota = 1;
            formaPago.Interes = 0;
            formaPago.Total = formaPago.Importe;

            var ventas = await this.repository.spPresupuestosPendiente(formaPago.VentaRapidaId);
            if (ventas == null)
            {
                return NotFound();
            }

            if (formaPago.Saldo < formaPago.Importe)
            {
                ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo");
            }

            //if (Convert.ToInt32(formaPago.TarjetaVenceAño) == DateTime.Now.Year && Convert.ToInt32(formaPago.TarjetaVenceMes) < DateTime.Now.Month)
            //{
            //    ModelState.AddModelError("TarjetaVenceMes", "Tarjeta Vencida");
            //}

            //if (Convert.ToInt32(formaPago.TarjetaVenceAño) < DateTime.Now.Year)
            //{
            //    ModelState.AddModelError("TarjetaVenceAño", "Tarjeta Vencida");

            //}

            if (ModelState.IsValid)
            {
                formaPago.Usuario = User.Identity.Name;
                formaPago.TarjetaEsDebito = false;

                await repository.spTarjeta(formaPago);

                return RedirectToAction(nameof(Pendiente), new { id = formaPago.VentaRapidaId });
            }


            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenPresupuesto(formaPago.VentaRapidaId);

            ViewBag.Años = new SelectList(combos.GetAños().OrderBy(x => x.Text), "Value", "Text", formaPago.TarjetaVenceAño);
            ViewBag.Meses = new SelectList(combos.GetMeses().OrderBy(x => x.Text), "Value", "Text", formaPago.TarjetaVenceMes);
            ViewBag.Tarjetas = new SelectList(cuotasRepository.GetEntidades(formaPago.FormaPagoId), "Value", "Text");

            return View(formaPago);
        }

        public async Task<IActionResult> TarjetaCredito(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spPresupuestosPendiente(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            var resumen = await repository.spResumenPresupuesto(id);
            ViewData["Resumen"] = resumen;

            var descuento = cuotasRepository.GetCuotaUno(fp);
            var recibo = new FormaPagoTarjetaDTO();
            recibo.ClienteId = ventas.ClienteId;
            recibo.VentaRapidaId = ventas.Id;
            recibo.FormaPagoId = fp;
            recibo.Saldo = resumen.SaldoAPagar;
            recibo.SaldoConDescuento = resumen.SaldoAPagar;

            ViewBag.Años = new SelectList(combos.GetAños().OrderBy(x => x.Text), "Value", "Text", recibo.TarjetaVenceAño);
            ViewBag.Meses = new SelectList(combos.GetMeses().OrderBy(x => x.Text), "Value", "Text", recibo.TarjetaVenceMes);
            ViewBag.Tarjetas = new SelectList(cuotasRepository.GetEntidades(fp), "Value", "Text");
            ViewBag.Cuotas = new SelectList(cuotasRepository.GetCuotas(fp, ""), "Value", "Text");

            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TarjetaCredito(FormaPagoTarjetaDTO formaPago)
        {
            var ventas = await this.repository.spPresupuestosPendiente(formaPago.VentaRapidaId);
            if (ventas == null)
            {
                return NotFound();
            }

            if (formaPago.Saldo < formaPago.Importe)
            {
                ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo");
            }

            if (formaPago.Cuota <= 0)
            {
                ModelState.AddModelError("Cuota", "El numero de cuota debe ser mayor que cero");
            }

            //if (Convert.ToInt32(formaPago.TarjetaVenceAño) == DateTime.Now.Year && Convert.ToInt32(formaPago.TarjetaVenceMes) < DateTime.Now.Month)
            //{
            //    ModelState.AddModelError("TarjetaVenceMes", "Tarjeta Vencida");
            //}

            //if (Convert.ToInt32(formaPago.TarjetaVenceAño) < DateTime.Now.Year)
            //{
            //    ModelState.AddModelError("TarjetaVenceAño", "Tarjeta Vencida");

            //}

            if (ModelState.IsValid)
            {
                formaPago.Usuario = User.Identity.Name;
                formaPago.TarjetaEsDebito = false;

                await repository.spTarjeta(formaPago);

                return RedirectToAction(nameof(Pendiente), new { id = formaPago.VentaRapidaId });
            }


            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenPresupuesto(formaPago.VentaRapidaId);

            ViewBag.Años = new SelectList(combos.GetAños().OrderBy(x => x.Text), "Value", "Text", formaPago.TarjetaVenceAño);
            ViewBag.Meses = new SelectList(combos.GetMeses().OrderBy(x => x.Text), "Value", "Text", formaPago.TarjetaVenceMes);
            ViewBag.Tarjetas = new SelectList(cuotasRepository.GetEntidades(formaPago.FormaPagoId), "Value", "Text");
            ViewBag.Cuotas = new SelectList(cuotasRepository.GetCuotas(formaPago.FormaPagoId, formaPago.TarjetaId), "Value", "Text");

            return View(formaPago);
        }

        public async Task<IActionResult> Dolar(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spPresupuestosPendiente(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);

            var resumen = await repository.spResumenPresupuesto(id);
            ViewData["Resumen"] = resumen;

            var descuento = cuotasRepository.GetCuotaUno(fp);
            var recibo = new FormaPagoDolarDTO();
            recibo.ClienteId = ventas.ClienteId;
            recibo.VentaRapidaId = ventas.Id;
            recibo.FormaPagoId = fp;
            recibo.Descuento = Math.Abs(descuento.Interes);
            recibo.Saldo = resumen.SaldoAPagar;
            recibo.SaldoConDescuento = resumen.SaldoAPagar * (1 - Math.Abs(descuento.Interes / 100));
            recibo.DolarCotizacion = await cotizacionRepository.spCotizacion();

            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dolar(FormaPagoDolarDTO formaPago)
        {
            var ventas = await this.repository.spPresupuestosPendiente(formaPago.VentaRapidaId);

            if (ventas == null)
            {
                return NotFound();
            }

            if (formaPago.Descuento == 0)//&& formaPago.Recargo == 0)
            {
                if (formaPago.Saldo < formaPago.Importe)
                {
                    ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo");
                }
            }
            else
            {
                if (formaPago.SaldoConDescuento < formaPago.Importe)
                {
                    ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo");
                }
            }

            if (ModelState.IsValid)
            {
                formaPago.Usuario = User.Identity.Name;
                await repository.spDolar(formaPago);

                return RedirectToAction(nameof(Pendiente), new { id = formaPago.VentaRapidaId });
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenPresupuesto(formaPago.VentaRapidaId);
            return View(formaPago);
        }

        public async Task<IActionResult> DeleteFormaPago(string id, string comp)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (comp == null)
            {
                return NotFound();
            }

            await repository.spDeleteFormaPago(id);
            return RedirectToAction(nameof(Pendiente), new { id = comp });
        }


        public async Task<IActionResult> ProductoEdit(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            return View(await this.repository.spPresupuestosDetalleId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductoEdit(PresupuestosDetalleDTO detalleDTO)
        {

            if (detalleDTO == null)
            {
                return RedirectToAction("Pendientes", "Presupuestos");
            }

            var detalle = new PresupuestosDetalle
            {
                Id = detalleDTO.Id,
                ProductoId = detalleDTO.ProductoId,
                Cantidad = detalleDTO.Cantidad,
                Precio = (decimal)detalleDTO.Precio,
                ProductoNombre = detalleDTO.ProductoNombre,
                UsuarioAlta = User.Identity.Name
            };

            await repository.spEditarProducto(detalle);
            return RedirectToAction("Pendiente", new { id = detalleDTO.PresupuestoId});
        }

    }

    
}
