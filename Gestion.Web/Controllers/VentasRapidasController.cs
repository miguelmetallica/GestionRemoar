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
    [Authorize(Roles = "Admin,VentasRapidas")]
    
    public class VentasRapidasController : Controller
    {
        private readonly IVentasRapidasRepository repository;

        private readonly IProductosRepository productosRepository;
        private readonly IClientesRepository clientesRepository;
        private readonly IPresupuestosDescuentosRepository descuentosRepository;
        private readonly IUserHelper userHelper;
        private readonly IConfiguracionesRepository configuraciones;
        private readonly ITiposResponsablesRepository tiposResponsables;
        private readonly IFormasPagosRepository formasPagosRepository;
        private readonly ICombosRepository combos;
        private readonly IComprobantesRepository comprobantesRepository;
        private readonly IFormasPagosCotizacionRepository cotizacionRepository;
        private readonly IFormasPagosCuotasRepository cuotasRepository;

        public VentasRapidasController(IVentasRapidasRepository repository,
            IProductosRepository productosRepository, 
            IClientesRepository clientesRepository,
            IPresupuestosDescuentosRepository descuentosRepository,
            IUserHelper userHelper,
            IConfiguracionesRepository configuraciones,
            ITiposResponsablesRepository tiposResponsables,
            IFormasPagosRepository formasPagosRepository,
            ICombosRepository combos,
            IComprobantesRepository comprobantesRepository,
            IFormasPagosCotizacionRepository cotizacionRepository,
            IFormasPagosCuotasRepository cuotasRepository
            )
        {
            this.repository = repository;
            this.productosRepository = productosRepository;
            this.clientesRepository = clientesRepository;
            this.descuentosRepository = descuentosRepository;
            this.userHelper = userHelper;
            this.configuraciones = configuraciones;
            this.tiposResponsables = tiposResponsables;
            this.formasPagosRepository = formasPagosRepository;
            this.combos = combos;
            this.comprobantesRepository = comprobantesRepository;
            this.cotizacionRepository = cotizacionRepository;
            this.cuotasRepository = cuotasRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await repository.spVentasRapidas();
            return View(model);
        }

        public async Task<IActionResult> VentasRapidasACobrar()
        {
            var model = await repository.spVentasRapidasACobrar();
            return View(model);
        }

        public async Task<IActionResult> Facturadas()
        {
            var model = await repository.spVentasRapidasFacturadas();
            return View(model);
        }

        public async Task<IActionResult> VentaRapida(string id)
        {
            var config = configuraciones.GetAll().Where(x => x.Configuracion == "CLIENTE_VENTA_CONTADO").FirstOrDefault();
            var ventasRapidas  = await this.repository.spVentasRapidas(id);
            if (ventasRapidas == null)
            {
                var ventasRapidasDto = new VentasRapidasDTO();
                ventasRapidasDto.ClienteId = config.Valor;
                ventasRapidasDto.Id = id;
                ventasRapidasDto.UsuarioAlta = User.Identity.Name;
                
                await repository.spInsertar(ventasRapidasDto);

                ventasRapidas = await this.repository.spVentasRapidas(id);                
            }

            var detalle = await this.repository.spDetalleVentaRapida(id);
            ViewData["Detalle"] = detalle;
            ViewData["FormasPagos"] = formasPagosRepository.GetAll().Where(x => x.Estado == true);
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);

            ViewData["Resumen"] = await repository.spResumenVentaRapida(id);

            return View(ventasRapidas);
        }

        public async Task<IActionResult> VentaRapidaACobrar(string id)
        {
            var ventasRapidas = await this.repository.spVentasRapidasACobrar(id);
            if (ventasRapidas == null)
            {
                return NotFound();
            }

            var detalle = await this.repository.spDetalleVentaRapida(id);
            ViewData["Detalle"] = detalle;
            ViewData["FormasPagos"] = formasPagosRepository.GetAll().Where(x => x.Estado == true);
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);

            ViewData["Resumen"] = await repository.spResumenVentaRapida(id);

            return View(ventasRapidas);
        }

        public async Task<IActionResult> VentaRapidaView(string id)
        {
            var ventasRapidas = await this.repository.spVentasRapidasFacturadas(id);
            if (ventasRapidas == null)
            {
                return NotFound();
            }

            var detalle = await this.repository.spDetalleVentaRapida(id);
            ViewData["Detalle"] = detalle;
            ViewData["FormasPagos"] = formasPagosRepository.GetAll().Where(x => x.Estado == true);
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);

            ViewData["Resumen"] = await repository.spResumenVentaRapida(id);

            return View(ventasRapidas);
        }

        public async Task<IActionResult> ClienteEditar(string id)
        {
            var ventas = await this.repository.spVentasRapidas(id);
            ViewBag.TiposResponsables = this.tiposResponsables.GetCombo();
            return View(ventas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClienteEditar(VentasRapidasDTO ventas)
        {
            if (ventas == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid) 
            {
                await this.repository.spClienteEditar(ventas);
                return RedirectToAction("VentaRapida", new { id = ventas.Id });
            }
                           
            ViewBag.TiposResponsables = this.tiposResponsables.GetCombo();
            return View(ventas);
        }

        public async Task<IActionResult> ClienteEditarACobrar(string id)
        {
            var ventas = await this.repository.spVentasRapidas(id);
            ViewBag.TiposResponsables = this.tiposResponsables.GetCombo();
            return View(ventas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClienteEditarACobrar(VentasRapidasDTO ventas)
        {
            if (ventas == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await this.repository.spClienteEditar(ventas);
                return RedirectToAction("VentaRapidaACobrar", new { id = ventas.Id });
            }

            ViewBag.TiposResponsables = this.tiposResponsables.GetCombo();
            return View(ventas);
        }
        public async Task<IActionResult> Descuento(string id)
        {
            var ventas = await this.repository.spVentasRapidas(id);

            if (ventas == null)
            {
                return RedirectToAction("Index", "VentasRapidas");
            }
            var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewData["Descuentos"] = descuentosRepository.GetAll()
                    .Where(x => x.Estado == true && x.UsuarioId == user.Id)
                    .OrderBy(x => x.Porcentaje);
            return View(ventas);
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

            var presup = await this.repository.spVentasRapidas(id2);

            if (presup == null)
            {
                return RedirectToAction("Index", "VentasRapidas");
            }

            await this.repository.spDescuentoAplica(id2, id, User.Identity.Name);
            return RedirectToAction("VentaRapida", new { id = id2 });
        }

        public async Task<IActionResult> DescuentoBorrar(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presup = await this.repository.spVentasRapidas(id);
            if (presup == null)
            {
                return RedirectToAction("Index", "VentasRapidas");
            }

            await this.repository.spDescuentoBorrar(id, User.Identity.Name);
            return RedirectToAction("VentaRapida", new { id });
        }

        public async Task<IActionResult> ProductoCreate(string id)
        {
            ViewData["productos"] = await productosRepository.spProductosVentaGet();

            var detalleDTO = new VentasRapidasDetalleDTO();
            detalleDTO.VentaRapidaId = id;
            return View(detalleDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductoCreate(VentasRapidasDetalleDTO detalleDTO)
        {
            if (ModelState.IsValid)
            {
                var prod_comodin = configuraciones.GetAll().Where(x => x.Configuracion == "PRODUCTO_COMODIN").FirstOrDefault();

                if (prod_comodin == null)
                {
                    return RedirectToAction("Index", "VentasRapidas");
                }

                var producto = await productosRepository.spProductosOneCodigo(prod_comodin.Valor);
                if (producto == null)
                {
                    return RedirectToAction("Index", "VentasRapidas");
                }

                detalleDTO.ProductoId = producto.Id;
                detalleDTO.ProductoCodigo = producto.Codigo;


                await repository.AddItemComodinAsync(detalleDTO, User.Identity.Name);
                return RedirectToAction("VentaRapida", new { id = detalleDTO.VentaRapidaId });
            }

            return View(detalleDTO);
        }
        public async Task<IActionResult> ProductoDelete(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            return View(await this.repository.spDetalleVentaRapidaId(id));
        }

        [HttpPost, ActionName("ProductoDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var detalleDTO = await repository.spDetalleVentaRapidaId(id);

            if (detalleDTO == null)
            {
                return RedirectToAction("Index", "VentasRapidas");
            }

            await repository.DeleteDetailAsync(detalleDTO.Id);
            return RedirectToAction("VentaRapida", new { id = detalleDTO.VentaRapidaId });
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

            var presup = await this.repository.spDetalleVentaRapidaId(id);

            if (presup == null)
            {
                return RedirectToAction("Index", "VentasRapidas");
            }

            await this.repository.ModifyCantidadesAsync(id, 1);
            return RedirectToAction("VentaRapida", new { id = id2 });
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

            var detalleDTO = await this.repository.spDetalleVentaRapidaId(id);

            if (detalleDTO == null)
            {
                return RedirectToAction("Index", "VentasRapidas");
            }

            var prod = detalleDTO;
            if (prod.Cantidad != 1)
            {
                await this.repository.ModifyCantidadesAsync(id, -1);
            }
            else
            {
                await this.repository.DeleteDetailAsync(id);
            }

            return RedirectToAction("VentaRapida", new { id = id2 });
        }

        public async Task<IActionResult> ProductoSeleccion(VentasRapidasDetalleDTO detalleDTO)
        {
            var ventas = await this.repository.spVentasRapidas(detalleDTO.VentaRapidaId);

            if (ventas == null)
            {
                return RedirectToAction("Index", "VentasRapidas");
            }

            var prod = await productosRepository.spProductosVentaGetOne(detalleDTO.ProductoId);
            detalleDTO.Cantidad = 1;
            detalleDTO.Precio = (decimal)prod.PrecioVenta;
            //detalleDTO.PrecioContado = (decimal)prod.PrecioContado;
            detalleDTO.ProductoCodigo = prod.Codigo;
            detalleDTO.ProductoNombre = prod.Producto;
            await repository.AddItemAsync(detalleDTO, User.Identity.Name);

            return RedirectToAction("VentaRapida", new { id = detalleDTO.VentaRapidaId});
        }

        public async Task<IActionResult> MediosPagos(string id)
        {
            var ventas = await this.repository.spVentasRapidas(id);

            if (ventas == null)
            {
                return RedirectToAction("Index", "VentasRapidas");
            }
            ViewData["FormasPagos"] = formasPagosRepository.GetAll().Where(x => x.Estado == true);
            return View(ventas);
        }

        public async Task<IActionResult> Efectivo(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spVentasRapidas(id,fp);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            var resumen = await repository.spResumenVentaRapida(id);
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
            var ventas = await this.repository.spVentasRapidas(formaPago.VentaRapidaId,formaPago.FormaPagoId);
            if (ventas == null)
            {
                return NotFound();
            }

            if (formaPago.Descuento == 0 )//&& formaPago.Recargo == 0)
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

                return RedirectToAction(nameof(VentaRapida), new { id = formaPago.VentaRapidaId });
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenVentaRapida(formaPago.VentaRapidaId);
            return View(formaPago);
        }

        public async Task<IActionResult> Otros(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spVentasRapidas(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["Ventas"] = ventas;
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);

            var recibo = new FormaPagoOtroDTO();
            recibo.ClienteId = ventas.ClienteId;
            recibo.VentaRapidaId = ventas.Id;
            recibo.FormaPagoId = fp;
            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Otros(FormaPagoOtroDTO formaPago)
        {
            var ventas = await this.repository.spVentasRapidas(formaPago.VentaRapidaId);
            if (ventas == null)
            {
                return NotFound();
            }

            var SaldoDto = await repository.spFormasPagos(formaPago.VentaRapidaId);
            if (SaldoDto == null)
            {
                return NotFound();
            }

            //var saldo = decimal.Round(ventas.Precio - (ventas.Precio * ventas.DescuentoPorcentaje / 100) - SaldoDto.Sum(x => x.ImporteLista), 2);
            //if (saldo == 0)
            //{
            //    ModelState.AddModelError("Importe", "El Saldo de la Cuenta es Cero");
            //}

            //if (saldo < formaPago.Importe)
            //{
            //    ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo de la Cuenta");
            //}

            if (ModelState.IsValid)
            {
                formaPago.Usuario = User.Identity.Name;
                await repository.spOtro(formaPago);

                return RedirectToAction(nameof(VentaRapida), new { id = formaPago.VentaRapidaId });
            }

            ViewData["Ventas"] = ventas;
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            return View(formaPago);
        }

        public async Task<IActionResult> TarjetaDebito(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spVentasRapidas(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            
            var resumen = await repository.spResumenVentaRapida(id);
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

            var ventas = await this.repository.spVentasRapidas(formaPago.VentaRapidaId);
            if (ventas == null)
            {
                return NotFound();
            }

            if (formaPago.Saldo < formaPago.Importe)
            {
                ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo");
            }

            if (Convert.ToInt32(formaPago.TarjetaVenceAño) == DateTime.Now.Year && Convert.ToInt32(formaPago.TarjetaVenceMes) < DateTime.Now.Month)
            {
                ModelState.AddModelError("TarjetaVenceMes", "Tarjeta Vencida");
            }

            if (Convert.ToInt32(formaPago.TarjetaVenceAño) < DateTime.Now.Year)
            {
                ModelState.AddModelError("TarjetaVenceAño", "Tarjeta Vencida");

            }

            if (ModelState.IsValid)
            {
                formaPago.Usuario = User.Identity.Name;
                formaPago.TarjetaEsDebito = false;

                await repository.spTarjeta(formaPago);

                return RedirectToAction(nameof(VentaRapida), new { id = formaPago.VentaRapidaId });
            }

            
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenVentaRapida(formaPago.VentaRapidaId);

            ViewBag.Años = new SelectList(combos.GetAños().OrderBy(x => x.Text), "Value", "Text", formaPago.TarjetaVenceAño);
            ViewBag.Meses = new SelectList(combos.GetMeses().OrderBy(x => x.Text), "Value", "Text", formaPago.TarjetaVenceMes);
            ViewBag.Tarjetas = new SelectList(cuotasRepository.GetEntidades(formaPago.FormaPagoId), "Value", "Text");

            return View(formaPago);
        }

        public async Task<IActionResult> Cheque(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spVentasRapidas(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            var resumen = await repository.spResumenVentaRapida(id);
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
            var ventas = await this.repository.spVentasRapidas(formaPago.VentaRapidaId);
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

                return RedirectToAction(nameof(VentaRapida), new { id = formaPago.VentaRapidaId });
            }

            
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenVentaRapida(formaPago.VentaRapidaId);
            ViewBag.Bancos = new SelectList(cuotasRepository.GetBancos(), "Value", "Text");
            return View(formaPago);
        }

        public async Task<IActionResult> TarjetaCredito(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spVentasRapidas(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            var resumen = await repository.spResumenVentaRapida(id);
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
            ViewBag.Cuotas = new SelectList(cuotasRepository.GetCuotas(fp,""), "Value", "Text");

            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TarjetaCredito(FormaPagoTarjetaDTO formaPago)
        {
            var ventas = await this.repository.spVentasRapidas(formaPago.VentaRapidaId);
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

            if (Convert.ToInt32(formaPago.TarjetaVenceAño) == DateTime.Now.Year && Convert.ToInt32(formaPago.TarjetaVenceMes) < DateTime.Now.Month)
            {
                ModelState.AddModelError("TarjetaVenceMes", "Tarjeta Vencida");
            }

            if (Convert.ToInt32(formaPago.TarjetaVenceAño) < DateTime.Now.Year)
            {
                ModelState.AddModelError("TarjetaVenceAño", "Tarjeta Vencida");

            }

            if (ModelState.IsValid)
            {
                formaPago.Usuario = User.Identity.Name;
                formaPago.TarjetaEsDebito = false;

                await repository.spTarjeta(formaPago);

                return RedirectToAction(nameof(VentaRapida), new { id = formaPago.VentaRapidaId });
            }

            
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenVentaRapida(formaPago.VentaRapidaId);

            ViewBag.Años = new SelectList(combos.GetAños().OrderBy(x => x.Text), "Value", "Text", formaPago.TarjetaVenceAño);
            ViewBag.Meses = new SelectList(combos.GetMeses().OrderBy(x => x.Text), "Value", "Text", formaPago.TarjetaVenceMes);
            ViewBag.Tarjetas = new SelectList(cuotasRepository.GetEntidades(formaPago.FormaPagoId), "Value", "Text");
            ViewBag.Cuotas = new SelectList(cuotasRepository.GetCuotas(formaPago.FormaPagoId, formaPago.TarjetaId), "Value", "Text");

            return View(formaPago);
        }

        public async Task<IActionResult> GenerarVenta(string id)
        {
            var detalle = await this.repository.spDetalleVentaRapida(id);
            if (detalle == null)
            {
                return NotFound();
            }

            var formasPagos = await repository.spFormasPagos(id);
            if (formasPagos == null)
            {
                return NotFound();
            }
            var ventas = new VentasRapidasDTO();
            if (ModelState.IsValid)
            {                
                ventas.Id = id;
                ventas.UsuarioAlta = User.Identity.Name;
                await repository.spGenerarVentaRapida(ventas);
                //await repository.spGenerarCobrazaVentaRapida(ventas);

                return RedirectToAction(nameof(VentaRapidaView) , new { id });
            }

            var det = await this.repository.spDetalleVentaRapida(id);
            ViewData["Detalle"] = detalle;
            ViewData["FormasPagos"] = formasPagosRepository.GetAll().Where(x => x.Estado == true);
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);

            return View(ventas);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await repository.spDelete(id);

            return RedirectToAction(nameof(Index));            
        }

        public async Task<IActionResult> DeleteACobrar(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await repository.spDelete(id);

            return RedirectToAction(nameof(VentasRapidasACobrar));
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
            return RedirectToAction(nameof(VentaRapida), new { id = comp });
        }
        public async Task<IActionResult> DeleteFormaPagoACobrar(string id, string comp)
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
            return RedirectToAction(nameof(VentaRapidaACobrar), new { id = comp });
        }

        public IActionResult ComprobanteImprimir(string id)
        {
            var ventasRapidas = this.repository.spVentasRapidasFacturadasPrint(id);
            if (ventasRapidas == null)
            {
                return NotFound();
            }

            var detalle = this.repository.spDetalleVentaRapidaPrint(id);
            ViewData["Detalle"] = detalle;
            //ViewData["FormasPagos"] = formasPagosRepository.GetAll().Where(x => x.Estado == true);
            ViewData["FormasPagosTMP"] = repository.spFormasPagosPrint(id);

            ViewData["Resumen"] = repository.spResumenVentaRapidaPrint(id);

            return View(ventasRapidas);
        }

        public async Task<IActionResult> Dolar(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spVentasRapidas(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);

            var resumen = await repository.spResumenVentaRapida(id);
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
            var ventas = await this.repository.spVentasRapidas(formaPago.VentaRapidaId);

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

                return RedirectToAction(nameof(VentaRapida), new { id = formaPago.VentaRapidaId });
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenVentaRapida(formaPago.VentaRapidaId);
            return View(formaPago);
        }

        public async Task<IActionResult> DolarACobrar(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spVentasRapidas(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);

            var resumen = await repository.spResumenVentaRapida(id);
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
        public async Task<IActionResult> DolarACobrar(FormaPagoDolarDTO formaPago)
        {
            var ventas = await this.repository.spVentasRapidas(formaPago.VentaRapidaId);

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

                return RedirectToAction(nameof(VentaRapidaACobrar), new { id = formaPago.VentaRapidaId });
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenVentaRapida(formaPago.VentaRapidaId);
            return View(formaPago);
        }


        public async Task<IActionResult> EfectivoACobrar(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spVentasRapidas(id, fp);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            var resumen = await repository.spResumenVentaRapida(id);
            ViewData["Resumen"] = resumen;

            var descuento = cuotasRepository.GetCuotaUno(fp);
            var recibo = new FormaPagoEfectivoDTO();
            recibo.ClienteId = ventas.ClienteId;
            recibo.VentaRapidaId = ventas.Id;
            recibo.FormaPagoId = fp;
            recibo.Descuento = Math.Abs(descuento.Interes);
            recibo.Saldo = resumen.SaldoAPagar;
            recibo.SaldoConDescuento = resumen.SaldoAPagar * (1 - Math.Abs(descuento.Interes / 100));
            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EfectivoACobrar(FormaPagoEfectivoDTO formaPago)
        {
            var ventas = await this.repository.spVentasRapidas(formaPago.VentaRapidaId, formaPago.FormaPagoId);
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

                return RedirectToAction(nameof(VentaRapidaACobrar), new { id = formaPago.VentaRapidaId });
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenVentaRapida(formaPago.VentaRapidaId);
            return View(formaPago);
        }

        public async Task<IActionResult> OtrosCobranza(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spVentasRapidas(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["Ventas"] = ventas;
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);

            var recibo = new FormaPagoOtroDTO();
            recibo.ClienteId = ventas.ClienteId;
            recibo.VentaRapidaId = ventas.Id;
            recibo.FormaPagoId = fp;
            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OtrosCobranza(FormaPagoOtroDTO formaPago)
        {
            var ventas = await this.repository.spVentasRapidas(formaPago.VentaRapidaId);
            if (ventas == null)
            {
                return NotFound();
            }

            var SaldoDto = await repository.spFormasPagos(formaPago.VentaRapidaId);
            if (SaldoDto == null)
            {
                return NotFound();
            }

            //var saldo = decimal.Round(ventas.Precio - (ventas.Precio * ventas.DescuentoPorcentaje / 100) - SaldoDto.Sum(x => x.ImporteLista), 2);
            //if (saldo == 0)
            //{
            //    ModelState.AddModelError("Importe", "El Saldo de la Cuenta es Cero");
            //}

            //if (saldo < formaPago.Importe)
            //{
            //    ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo de la Cuenta");
            //}

            if (ModelState.IsValid)
            {
                formaPago.Usuario = User.Identity.Name;
                await repository.spOtro(formaPago);

                return RedirectToAction(nameof(VentaRapidaACobrar), new { id = formaPago.VentaRapidaId });
            }

            ViewData["Ventas"] = ventas;
            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            return View(formaPago);
        }

        public async Task<IActionResult> TarjetaDebitoACobrar(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spVentasRapidas(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);

            var resumen = await repository.spResumenVentaRapida(id);
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
        public async Task<IActionResult> TarjetaDebitoACobrar(FormaPagoTarjetaDTO formaPago)
        {
            formaPago.Cuota = 1;
            formaPago.Interes = 0;
            formaPago.Total = formaPago.Importe;

            var ventas = await this.repository.spVentasRapidas(formaPago.VentaRapidaId);
            if (ventas == null)
            {
                return NotFound();
            }

            if (formaPago.Saldo < formaPago.Importe)
            {
                ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo");
            }

            if (Convert.ToInt32(formaPago.TarjetaVenceAño) == DateTime.Now.Year && Convert.ToInt32(formaPago.TarjetaVenceMes) < DateTime.Now.Month)
            {
                ModelState.AddModelError("TarjetaVenceMes", "Tarjeta Vencida");
            }

            if (Convert.ToInt32(formaPago.TarjetaVenceAño) < DateTime.Now.Year)
            {
                ModelState.AddModelError("TarjetaVenceAño", "Tarjeta Vencida");

            }

            if (ModelState.IsValid)
            {
                formaPago.Usuario = User.Identity.Name;
                formaPago.TarjetaEsDebito = false;

                await repository.spTarjeta(formaPago);

                return RedirectToAction(nameof(VentaRapidaACobrar), new { id = formaPago.VentaRapidaId });
            }


            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenVentaRapida(formaPago.VentaRapidaId);

            ViewBag.Años = new SelectList(combos.GetAños().OrderBy(x => x.Text), "Value", "Text", formaPago.TarjetaVenceAño);
            ViewBag.Meses = new SelectList(combos.GetMeses().OrderBy(x => x.Text), "Value", "Text", formaPago.TarjetaVenceMes);
            ViewBag.Tarjetas = new SelectList(cuotasRepository.GetEntidades(formaPago.FormaPagoId), "Value", "Text");

            return View(formaPago);
        }

        public async Task<IActionResult> TarjetaCreditoACobrar(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spVentasRapidas(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            var resumen = await repository.spResumenVentaRapida(id);
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
        public async Task<IActionResult> TarjetaCreditoACobrar(FormaPagoTarjetaDTO formaPago)
        {
            var ventas = await this.repository.spVentasRapidas(formaPago.VentaRapidaId);
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

            if (Convert.ToInt32(formaPago.TarjetaVenceAño) == DateTime.Now.Year && Convert.ToInt32(formaPago.TarjetaVenceMes) < DateTime.Now.Month)
            {
                ModelState.AddModelError("TarjetaVenceMes", "Tarjeta Vencida");
            }

            if (Convert.ToInt32(formaPago.TarjetaVenceAño) < DateTime.Now.Year)
            {
                ModelState.AddModelError("TarjetaVenceAño", "Tarjeta Vencida");

            }

            if (ModelState.IsValid)
            {
                formaPago.Usuario = User.Identity.Name;
                formaPago.TarjetaEsDebito = false;

                await repository.spTarjeta(formaPago);

                return RedirectToAction(nameof(VentaRapidaACobrar), new { id = formaPago.VentaRapidaId });
            }


            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenVentaRapida(formaPago.VentaRapidaId);

            ViewBag.Años = new SelectList(combos.GetAños().OrderBy(x => x.Text), "Value", "Text", formaPago.TarjetaVenceAño);
            ViewBag.Meses = new SelectList(combos.GetMeses().OrderBy(x => x.Text), "Value", "Text", formaPago.TarjetaVenceMes);
            ViewBag.Tarjetas = new SelectList(cuotasRepository.GetEntidades(formaPago.FormaPagoId), "Value", "Text");
            ViewBag.Cuotas = new SelectList(cuotasRepository.GetCuotas(formaPago.FormaPagoId, formaPago.TarjetaId), "Value", "Text");

            return View(formaPago);
        }

        public async Task<IActionResult> ChequeACobrar(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await repository.spVentasRapidas(id);
            if (ventas == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(id);
            var resumen = await repository.spResumenVentaRapida(id);
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
        public async Task<IActionResult> ChequeACobrar(FormaPagoChequeDTO formaPago)
        {
            var ventas = await this.repository.spVentasRapidas(formaPago.VentaRapidaId);
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

                return RedirectToAction(nameof(VentaRapidaACobrar), new { id = formaPago.VentaRapidaId });
            }


            ViewData["FormasPagosTMP"] = await repository.spFormasPagos(formaPago.VentaRapidaId);
            ViewData["Resumen"] = await repository.spResumenVentaRapida(formaPago.VentaRapidaId);
            ViewBag.Bancos = new SelectList(cuotasRepository.GetBancos(), "Value", "Text");
            return View(formaPago);
        }
    }
}
