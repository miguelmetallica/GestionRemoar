using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly IClientesRepository repository;
        private readonly IUserHelper userHelper;
        private readonly ITiposDocumentosRepository tiposDocumentos;
        private readonly IProvinciasRepository provincias;
        private readonly IFactoryConnection factoryConnection;
        private readonly IComprobantesRepository comprobantesRepository;
        private readonly IFormasPagosRepository formasPagosRepository;
        private readonly ITiposResponsablesRepository tiposResponsables;
        private readonly ICombosRepository combos;
        private readonly IPresupuestosRepository presupuestos;
        private readonly IClientesCategoriasRepository clientesCategorias;

        public ClientesController(IClientesRepository repository, IUserHelper userHelper, 
                ITiposDocumentosRepository tiposDocumentos, IProvinciasRepository provincias,
                IFactoryConnection factoryConnection,IComprobantesRepository comprobantesRepository,
                IFormasPagosRepository formasPagosRepository, ITiposResponsablesRepository tiposResponsables,
                ICombosRepository combos,IPresupuestosRepository presupuestos,IClientesCategoriasRepository clientesCategorias)
        {
            this.repository = repository;
            this.userHelper = userHelper;
            this.tiposDocumentos = tiposDocumentos;
            this.provincias = provincias;
            this.factoryConnection = factoryConnection;
            this.comprobantesRepository = comprobantesRepository;
            this.formasPagosRepository = formasPagosRepository;
            this.tiposResponsables = tiposResponsables;
            this.combos = combos;
            this.presupuestos = presupuestos;
            this.clientesCategorias = clientesCategorias;
        }

        
        public IActionResult Index()
        {
            return View(this.repository.GetAll());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await this.repository.spCliente(id);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Comprobantes"] = await comprobantesRepository.spComprobantes(id);

            return View(cliente.FirstOrDefault());            
        }

        public IActionResult Create()
        {
            ViewBag.TiposDocumentos = this.tiposDocumentos.GetCombo();
            ViewBag.Provincias = this.provincias.GetCombo();
            ViewBag.TiposResponsables = this.tiposResponsables.GetCombo();
            ViewBag.Categorias = this.clientesCategorias.GetCombo();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientesFisicoAdd clientes)
        {
            if (ModelState.IsValid)
            {
                var doc = await this.repository.ExistNroDocAsync("", clientes.TipoDocumentoId, clientes.NroDocumento);
                if (doc)
                {
                    ModelState.AddModelError("NroDocumento", "El Tipo y Nro de Documento ya esta cargado en la base de datos");                                       
                }
                
                if (clientes.CuilCuit != null)
                {
                    var cuitcuil = await this.repository.ExistCuitCuilAsync("", clientes.CuilCuit);
                    if (cuitcuil)
                    {
                        ModelState.AddModelError("CuilCuit", "El Cuil ya esta cargado en la base de datos");
                    }
                }

                if (ModelState.IsValid)
                {
                    clientes.Id = Guid.NewGuid().ToString();
                    clientes.UsuarioAlta =User.Identity.Name;
                    await repository.spInsertar(clientes);

                    return RedirectToAction(nameof(Details), new { id = clientes.Id });
                }
            }

            ViewBag.TiposDocumentos = this.tiposDocumentos.GetCombo();
            ViewBag.Provincias = this.provincias.GetCombo();
            ViewBag.TiposResponsables = this.tiposResponsables.GetCombo();
            ViewBag.Categorias = this.clientesCategorias.GetCombo();

            return View(clientes);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await this.repository.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewBag.TiposDocumentos = this.tiposDocumentos.GetCombo();
            ViewBag.Provincias = this.provincias.GetCombo();
            ViewBag.TiposResponsables = this.tiposResponsables.GetCombo();
            ViewBag.Categorias = this.clientesCategorias.GetCombo();

            return View(toClienteEdit(cliente));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientesFisicoEdit clientes)
        {
            var doc = await this.repository.ExistNroDocAsync(clientes.Id, clientes.TipoDocumentoId, clientes.NroDocumento);
            if (doc)
            {
                ModelState.AddModelError("NroDocumento", "El Tipo y Nro de Documento ya esta cargado en la base de datos");
            }

            if (clientes.CuilCuit != null)
            {
                var cuitcuil = await this.repository.ExistCuitCuilAsync(clientes.Id, clientes.CuilCuit);
                if (cuitcuil)
                {
                    ModelState.AddModelError("CuilCuit", "El Cuil ya esta cargado en la base de datos");
                }
            }            

            if (ModelState.IsValid)
            {
                await repository.spEditar(clientes);

                return RedirectToAction(nameof(Details), new { id = clientes.Id });
            }

            ViewBag.TiposDocumentos = this.tiposDocumentos.GetCombo();
            ViewBag.Provincias = this.provincias.GetCombo();
            ViewBag.TiposResponsables = this.tiposResponsables.GetCombo();
            ViewBag.Categorias = this.clientesCategorias.GetCombo();

            return View(clientes);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await this.repository.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            cliente.Estado = !cliente.Estado;
            await this.repository.DeleteAsync(cliente);
            return RedirectToAction(nameof(Index));
        }

        public ClientesFisicoEdit toClienteEdit(Clientes clientes)
        {
            var cliente = new ClientesFisicoEdit
            {
                Id = clientes.Id,
                Codigo = clientes.Codigo,
                Apellido = clientes.Apellido,
                Nombre = clientes.Nombre,                
                TipoDocumentoId = clientes.TipoDocumentoId,
                NroDocumento = clientes.NroDocumento,
                CuilCuit = clientes.CuilCuit,
                FechaNacimiento = clientes.FechaNacimiento,
                ProvinciaId = clientes.ProvinciaId,
                Localidad = clientes.Localidad,
                CodigoPostal = clientes.CodigoPostal,
                Calle = clientes.Calle,
                CalleNro = clientes.CalleNro,
                PisoDpto = clientes.PisoDpto,
                OtrasReferencias = clientes.OtrasReferencias,
                Telefono = clientes.Telefono,
                Celular = clientes.Celular,
                Email = clientes.Email,
                TipoResponsableId = clientes.TipoResponsableId,
                CategoriaId = clientes.CategoriaId
            };
            return cliente;
        }

        public Clientes toCliente(ClientesFisicoAdd clientes)
        {
            var cliente = new Clientes
            {
                Id = clientes.Id,
                Apellido = clientes.Apellido,
                Nombre = clientes.Nombre,
                RazonSocial = clientes.Apellido + ' ' + clientes.Nombre,
                TipoDocumentoId = clientes.TipoDocumentoId,
                NroDocumento = clientes.NroDocumento,
                CuilCuit = clientes.CuilCuit,
                FechaNacimiento = clientes.FechaNacimiento,
                ProvinciaId = clientes.ProvinciaId,
                Localidad = clientes.Localidad,
                CodigoPostal = clientes.CodigoPostal,
                Calle = clientes.Calle,
                CalleNro = clientes.CalleNro,
                PisoDpto = clientes.PisoDpto,
                OtrasReferencias = clientes.OtrasReferencias,
                Telefono = clientes.Telefono,
                Celular = clientes.Celular,
                Email = clientes.Email,
                TipoResponsableId = clientes.TipoResponsableId,
                CategoriaId = clientes.CategoriaId
            };
            return cliente;
        }

        public Clientes toCliente(ClientesFisicoEdit clientes)
        {
            var cliente = new Clientes
            {
                Id = clientes.Id,
                Codigo = clientes.Codigo,
                Apellido = clientes.Apellido,
                Nombre = clientes.Nombre,
                RazonSocial = clientes.Apellido + ' ' + clientes.Nombre,
                TipoDocumentoId = clientes.TipoDocumentoId,
                NroDocumento = clientes.NroDocumento,
                CuilCuit = clientes.CuilCuit,
                FechaNacimiento = clientes.FechaNacimiento,
                ProvinciaId = clientes.ProvinciaId,
                Localidad = clientes.Localidad,
                CodigoPostal = clientes.CodigoPostal,
                Calle = clientes.Calle,
                CalleNro = clientes.CalleNro,
                PisoDpto = clientes.PisoDpto,
                OtrasReferencias = clientes.OtrasReferencias,
                Telefono = clientes.Telefono,
                Celular = clientes.Celular,
                Email = clientes.Email,
                TipoResponsableId = clientes.TipoResponsableId,
                CategoriaId = clientes.CategoriaId
            };
            return cliente;
        }

        public async Task<int> spNuevo(ClientesFisicoAdd item)
        {
            try
            {                
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ClientesInsertar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
                        oCmd.Parameters.AddWithValue("@Apellido", item.Apellido);
                        oCmd.Parameters.AddWithValue("@Nombre", item.Nombre);
                        oCmd.Parameters.AddWithValue("@TipoDocumentoId", item.TipoDocumentoId);
                        oCmd.Parameters.AddWithValue("@NroDocumento", item.NroDocumento);
                        oCmd.Parameters.AddWithValue("@CuilCuit", item.CuilCuit);
                        oCmd.Parameters.AddWithValue("@FechaNacimiento", item.FechaNacimiento);
                        oCmd.Parameters.AddWithValue("@ProvinciaId", item.ProvinciaId);
                        oCmd.Parameters.AddWithValue("@Localidad", item.Localidad);
                        oCmd.Parameters.AddWithValue("@CodigoPostal", item.CodigoPostal);
                        oCmd.Parameters.AddWithValue("@Calle", item.Calle);
                        oCmd.Parameters.AddWithValue("@CalleNro", item.CalleNro);
                        oCmd.Parameters.AddWithValue("@PisoDpto", item.PisoDpto);
                        oCmd.Parameters.AddWithValue("@OtrasReferencias", item.OtrasReferencias);
                        oCmd.Parameters.AddWithValue("@Telefono", item.Telefono);
                        oCmd.Parameters.AddWithValue("@Celular", item.Celular);
                        oCmd.Parameters.AddWithValue("@Email", item.Email);
                        oCmd.Parameters.AddWithValue("@Estado", true);
                        oCmd.Parameters.AddWithValue("@TipoResponsableId", item.TipoResponsableId);
                        oCmd.Parameters.AddWithValue("@CategoriaId", item.CategoriaId);
                        oCmd.Parameters.AddWithValue("@Usuario", User.Identity.Name);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }                
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<int> spEditar(ClientesFisicoEdit item)
        {
            try
            {
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "ClientesEditar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", item.Id);
                        oCmd.Parameters.AddWithValue("@Apellido", item.Apellido);
                        oCmd.Parameters.AddWithValue("@Nombre", item.Nombre);
                        oCmd.Parameters.AddWithValue("@TipoDocumentoId", item.TipoDocumentoId);
                        oCmd.Parameters.AddWithValue("@NroDocumento", item.NroDocumento);
                        oCmd.Parameters.AddWithValue("@CuilCuit", item.CuilCuit);
                        oCmd.Parameters.AddWithValue("@FechaNacimiento", item.FechaNacimiento);
                        oCmd.Parameters.AddWithValue("@ProvinciaId", item.ProvinciaId);
                        oCmd.Parameters.AddWithValue("@Localidad", item.Localidad);
                        oCmd.Parameters.AddWithValue("@CodigoPostal", item.CodigoPostal);
                        oCmd.Parameters.AddWithValue("@Calle", item.Calle);
                        oCmd.Parameters.AddWithValue("@CalleNro", item.CalleNro);
                        oCmd.Parameters.AddWithValue("@PisoDpto", item.PisoDpto);
                        oCmd.Parameters.AddWithValue("@OtrasReferencias", item.OtrasReferencias);
                        oCmd.Parameters.AddWithValue("@Telefono", item.Telefono);
                        oCmd.Parameters.AddWithValue("@Celular", item.Celular);
                        oCmd.Parameters.AddWithValue("@Email", item.Email);
                        oCmd.Parameters.AddWithValue("@Estado", true);
                        oCmd.Parameters.AddWithValue("@TipoResponsableId", item.TipoResponsableId);
                        oCmd.Parameters.AddWithValue("@CategoriaId", item.CategoriaId);
                        oCmd.Parameters.AddWithValue("@Usuario", User.Identity.Name);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<IActionResult> Recibo(string id)
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

            var cliente = await this.repository.spCliente(comprobante.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }
            
            ViewData["FormasPagosTMP"] = await comprobantesRepository.spComprobantesTmpFormasPagos(comprobante.Id);
            ViewData["Saldo"] = comprobante;
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagos"] = formasPagosRepository.GetAll().Where(x=> x.Estado == true);
            
            var recibo = new ComprobantesReciboDTO();
            recibo.ClienteId = comprobante.ClienteId;
            recibo.ComprobanteId = comprobante.Id;

            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recibo(ComprobantesReciboDTO reciboDTO)
        {
            var cliente = await this.repository.spCliente(reciboDTO.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }
            
            var formasPagos = await comprobantesRepository.spComprobantesTmpFormasPagos(reciboDTO.ComprobanteId);
            var SaldoDto = await comprobantesRepository.spComprobante(reciboDTO.ComprobanteId);

            if (SaldoDto.Saldo < 0)
            {
                ModelState.AddModelError("Importe", "El Saldo de la Cuenta es Cero");                
            }
            
            if (SaldoDto.Saldo < reciboDTO.Importe)
            {
                ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo de la Cuenta");
            }

            if (ModelState.IsValid)
            {
                reciboDTO.Usuario = User.Identity.Name; 
                await comprobantesRepository.spRecibo(reciboDTO);

                return RedirectToAction(nameof(ComprobanteDetalle),new { id = reciboDTO.ComprobanteId });
            }

            ViewData["FormasPagosTMP"] = formasPagos;
            ViewData["Saldo"] = SaldoDto;
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagos"] = formasPagosRepository.GetAll().Where(x => x.Estado == true);

            return View(reciboDTO);
        }

        public async Task<IActionResult> Efectivo(string id, string fp)
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

            if (fp == null)
            {
                return NotFound();
            }

            var cliente = await this.repository.spCliente(comprobante.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Saldo"] = comprobante;
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagosTMP"] = await comprobantesRepository.spComprobantesTmpFormasPagos(comprobante.ClienteId);

            var recibo = new ComprobantesEfectivoDTO();
            recibo.ClienteId = comprobante.ClienteId;
            recibo.ComprobanteId = comprobante.Id;
            recibo.FormaPagoId = fp;
            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Efectivo(ComprobantesEfectivoDTO reciboDTO)
        {
            var SaldoDto = await comprobantesRepository.spComprobante(reciboDTO.ComprobanteId);
            if (SaldoDto == null)
            {
                return NotFound();
            }
            var SaldoTmp = await comprobantesRepository.spComprobantesTmpFormasPagos(reciboDTO.ClienteId);
            if (SaldoTmp == null)
            {
                return NotFound();
            }

            if (SaldoDto.Saldo - SaldoTmp.Sum(x => x.Total) == 0)
            {
                ModelState.AddModelError("Importe", "El Saldo de la Cuenta es Cero");
            }

            if (SaldoDto.Saldo - SaldoTmp.Sum(x => x.Total) < reciboDTO.Importe)
            {
                ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo de la Cuenta");
            }

            if (ModelState.IsValid)
            {
                reciboDTO.Usuario = User.Identity.Name;
                await comprobantesRepository.spEfectivo(reciboDTO);

                return RedirectToAction(nameof(Recibo), new { id = reciboDTO.ComprobanteId });
            }

            ViewData["Saldo"] = SaldoDto;
            var cliente = await this.repository.spCliente(reciboDTO.ClienteId);
            ViewData["Cliente"] = cliente.FirstOrDefault();

            ViewData["FormasPagosTMP"] = SaldoTmp;

            return View(reciboDTO);
        }

        public async Task<IActionResult> Otro(string id, string fp)
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

            if (fp == null)
            {
                return NotFound();
            }

            var cliente = await this.repository.spCliente(comprobante.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Saldo"] = comprobante;
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagosTMP"] = await comprobantesRepository.spComprobantesTmpFormasPagos(comprobante.ClienteId);

            var recibo = new ComprobantesOtroDTO();
            recibo.ClienteId = comprobante.ClienteId;
            recibo.ComprobanteId = comprobante.Id;
            recibo.FormaPagoId = fp;
            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Otro(ComprobantesOtroDTO reciboDTO)
        {
            var SaldoDto = await comprobantesRepository.spComprobante(reciboDTO.ComprobanteId);
            if (SaldoDto == null)
            {
                return NotFound();
            }
            var SaldoTmp = await comprobantesRepository.spComprobantesTmpFormasPagos(reciboDTO.ClienteId);
            if (SaldoTmp == null)
            {
                return NotFound();
            }

            if (SaldoDto.Saldo - SaldoTmp.Sum(x => x.Total) == 0)
            {
                ModelState.AddModelError("Importe", "El Saldo de la Cuenta es Cero");                
            }

            if (SaldoDto.Saldo - SaldoTmp.Sum(x => x.Total) < reciboDTO.Importe)
            {
                ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo de la Cuenta");
            }

            if (ModelState.IsValid)
            {
            
                reciboDTO.Usuario = User.Identity.Name;
                await comprobantesRepository.spOtro(reciboDTO);

                return RedirectToAction(nameof(Recibo), new { id = reciboDTO.ComprobanteId });
            }

            ViewData["Saldo"] = SaldoDto;
            var cliente = await this.repository.spCliente(reciboDTO.ClienteId);
            ViewData["Cliente"] = cliente.FirstOrDefault();

            ViewData["FormasPagosTMP"] = SaldoTmp;

            return View(reciboDTO);
        }

        public async Task<IActionResult> TarjetaDebito(string id, string fp)
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

            if (fp == null)
            {
                return NotFound();
            }

            var cliente = await this.repository.spCliente(comprobante.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Saldo"] = comprobante;
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagosTMP"] = await comprobantesRepository.spComprobantesTmpFormasPagos(comprobante.ClienteId);

            ViewBag.Años = new SelectList(combos.GetAños().OrderBy(x => x.Text), "Value", "Text"); 
            ViewBag.Meses = new SelectList(combos.GetMeses().OrderBy(x => x.Text), "Value", "Text");

            var recibo = new ComprobantesTarjetaDTO();
            recibo.ClienteId = comprobante.ClienteId;
            recibo.ComprobanteId = comprobante.Id;
            
            recibo.FormaPagoId = fp;

            recibo.Cuota = 1;
            recibo.Interes = 0;
            recibo.Total = 0;

            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TarjetaDebito(ComprobantesTarjetaDTO reciboDTO)
        {
            reciboDTO.Cuota = 1;
            reciboDTO.Interes = 0;
            reciboDTO.Total = reciboDTO.Importe;


            var SaldoDto = await comprobantesRepository.spComprobante(reciboDTO.ComprobanteId);
            if (SaldoDto == null)
            {
                return NotFound();
            }
            var SaldoTmp = await comprobantesRepository.spComprobantesTmpFormasPagos(reciboDTO.ComprobanteId);
            if (SaldoTmp == null)
            {
                return NotFound();
            }

            if (SaldoDto.Saldo - SaldoTmp.Sum(x => x.Total) == 0)
            {
                ModelState.AddModelError("Importe", "El Saldo de la Cuenta es Cero");                
            }

            if(Convert.ToInt32(reciboDTO.TarjetaVenceAño) == DateTime.Now.Year && Convert.ToInt32(reciboDTO.TarjetaVenceMes) < DateTime.Now.Month)
            {
                ModelState.AddModelError("TarjetaVenceMes", "Tarjeta Vencida");                
            }

            if (Convert.ToInt32(reciboDTO.TarjetaVenceAño) < DateTime.Now.Year)
            {
                ModelState.AddModelError("TarjetaVenceAño", "Tarjeta Vencida");
                
            }
            if (SaldoDto.Saldo - SaldoTmp.Sum(x => x.Total) < reciboDTO.Importe)
            {
                ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo de la Cuenta");
            }

            if (ModelState.IsValid)
            {
                reciboDTO.Usuario = User.Identity.Name;
                reciboDTO.TarjetaEsDebito = true;
                reciboDTO.Cuota = 1;
                reciboDTO.Interes= 0;
                reciboDTO.Total = reciboDTO.Importe;

                await comprobantesRepository.spTarjeta(reciboDTO);

                return RedirectToAction(nameof(Recibo), new { id = reciboDTO.ComprobanteId });
            }
            
            ViewData["Saldo"] = SaldoDto;
            var cliente = await this.repository.spCliente(reciboDTO.ClienteId);
            ViewData["Cliente"] = cliente.FirstOrDefault();

            ViewData["FormasPagosTMP"] = SaldoTmp;

            ViewBag.Años = new SelectList(combos.GetAños().OrderBy(x => x.Text), "Value", "Text", reciboDTO.TarjetaVenceAño);
            ViewBag.Meses = new SelectList(combos.GetMeses().OrderBy(x => x.Text), "Value", "Text", reciboDTO.TarjetaVenceMes);

            return View(reciboDTO);
        }

        public async Task<IActionResult> TarjetaCredito(string id, string fp)
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

            if (fp == null)
            {
                return NotFound();
            }

            var cliente = await this.repository.spCliente(comprobante.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Saldo"] = comprobante;
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagosTMP"] = await comprobantesRepository.spComprobantesTmpFormasPagos(comprobante.ClienteId);


            var recibo = new ComprobantesTarjetaDTO();
            recibo.ClienteId = comprobante.ClienteId;
            recibo.ComprobanteId = comprobante.Id;

            recibo.FormaPagoId = fp;
            
            ViewBag.Años = new SelectList(combos.GetAños().OrderBy(x => x.Text), "Value", "Text", recibo.TarjetaVenceAño);
            ViewBag.Meses = new SelectList(combos.GetMeses().OrderBy(x => x.Text), "Value", "Text", recibo.TarjetaVenceMes);

            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TarjetaCredito(ComprobantesTarjetaDTO reciboDTO)
        {
            var SaldoDto = await comprobantesRepository.spComprobante(reciboDTO.ComprobanteId);
            if (SaldoDto == null)
            {
                return NotFound();
            }
            var SaldoTmp = await comprobantesRepository.spComprobantesTmpFormasPagos(reciboDTO.ClienteId);
            if (SaldoTmp == null)
            {
                return NotFound();
            }

            if (SaldoDto.Saldo - SaldoTmp.Sum(x => x.Total) == 0)
            {
                ModelState.AddModelError("Importe", "El Saldo de la Cuenta es Cero");
            }

            if (SaldoDto.Saldo - SaldoTmp.Sum(x => x.Total) < reciboDTO.Importe)
            {
                ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo de la Cuenta");
            }

            if (Convert.ToInt32(reciboDTO.TarjetaVenceAño) == DateTime.Now.Year && Convert.ToInt32(reciboDTO.TarjetaVenceMes) < DateTime.Now.Month)
            {
                ModelState.AddModelError("TarjetaVenceMes", "Tarjeta Vencida");
            }

            if (Convert.ToInt32(reciboDTO.TarjetaVenceAño) < DateTime.Now.Year)
            {
                ModelState.AddModelError("TarjetaVenceAño", "Tarjeta Vencida");

            }

            if (ModelState.IsValid)
            {
                reciboDTO.Usuario = User.Identity.Name;
                reciboDTO.TarjetaEsDebito = false;                

                await comprobantesRepository.spTarjeta(reciboDTO);

                return RedirectToAction(nameof(Recibo), new { id = reciboDTO.ComprobanteId });
            }

            ViewData["Saldo"] = SaldoDto;
            var cliente = await this.repository.spCliente(reciboDTO.ClienteId);
            ViewData["Cliente"] = cliente.FirstOrDefault();

            ViewData["FormasPagosTMP"] = SaldoTmp;

            ViewBag.Años = new SelectList(combos.GetAños().OrderBy(x => x.Text), "Value", "Text", reciboDTO.TarjetaVenceAño);
            ViewBag.Meses = new SelectList(combos.GetMeses().OrderBy(x => x.Text), "Value", "Text", reciboDTO.TarjetaVenceMes);

            return View(reciboDTO);
        }

        public async Task<IActionResult> Cheque(string id, string fp)
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

            if (fp == null)
            {
                return NotFound();
            }

            var cliente = await this.repository.spCliente(comprobante.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Saldo"] = comprobante;
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagosTMP"] = await comprobantesRepository.spComprobantesTmpFormasPagos(comprobante.ClienteId);

            var recibo = new ComprobantesChequeDTO();
            recibo.ClienteId = comprobante.ClienteId;
            recibo.ComprobanteId = comprobante.Id;

            recibo.FormaPagoId = fp;
            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cheque(ComprobantesChequeDTO reciboDTO)
        {
            var SaldoDto = await comprobantesRepository.spComprobante(reciboDTO.ComprobanteId);
            if (SaldoDto == null)
            {
                return NotFound();
            }
            var SaldoTmp = await comprobantesRepository.spComprobantesTmpFormasPagos(reciboDTO.ClienteId);
            if (SaldoTmp == null)
            {
                return NotFound();
            }

            if (SaldoDto.Saldo - SaldoTmp.Sum(x => x.Total) == 0)
            {
                ModelState.AddModelError("Importe", "El Saldo de la Cuenta es Cero");
            }

            if (SaldoDto.Saldo - SaldoTmp.Sum(x => x.Total) < reciboDTO.Importe)
            {
                ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo de la Cuenta");
            }

            if (ModelState.IsValid)
            {
                reciboDTO.Usuario = User.Identity.Name;
                await comprobantesRepository.spCheque(reciboDTO);

                return RedirectToAction(nameof(Recibo), new { id = reciboDTO.ComprobanteId });
            }

            ViewData["Saldo"] = SaldoDto;
            var cliente = await this.repository.spCliente(reciboDTO.ClienteId);
            ViewData["Cliente"] = cliente.FirstOrDefault();

            ViewData["FormasPagosTMP"] = SaldoTmp;

            return View(reciboDTO);
        }

        public async Task<IActionResult> DeleteFormaPago(string id,string comp)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (comp == null)
            {
                return NotFound();
            }

            await comprobantesRepository.spDeleteFormaPago(id);
            return RedirectToAction(nameof(Recibo) , new { id = comp });
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

            var cliente = await this.repository.spCliente(comprobante.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Comprobantes"] = comprobante;
            ViewData["Imputaciones"] = await comprobantesRepository.spComprobanteImputacion(id);
            ViewData["FormasPagos"] = await comprobantesRepository.spComprobantesFormasPagos(id);            
            ViewData["Detalles"] = await presupuestos.spPresupuestosAprobado(comprobante.PresupuestoId);

            return View(cliente.FirstOrDefault());
        }

        public async Task<IActionResult> ReciboImprimir(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var CC = await comprobantesRepository.spComprobante(id);
            if (CC == null)
            {
                return NotFound();
            }

            //var cliente = await this.repository.spCliente(CC.ClienteId);
            //if (cliente == null)
            //{
            //    return NotFound();
            //}
            
            //ViewData["CCIMPUTACION"] = await comprobantesRepository.spComprobanteImputacion(comp);
            ViewData["Detalle"] = await comprobantesRepository.spComprobantesFormasPagosImprimir(id);
            //ViewData["CC"] = CC;
            //ViewData["CCDETALLE"] = await presupuestos.spPresupuestosAprobado(CC.PresupuestoId);


            //ViewData["PRESUPUESTO"] = await comprobantesRepository.spComprobante(comp);

            return View(CC);
        }

        public async Task<IActionResult> ReciboAnular(string id, string comp)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (comp == null)
            {
                return NotFound();
            }

            var recibos = await comprobantesRepository.spComprobante(id);

            var cliente = await this.repository.spCliente(recibos.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Cliente"] = cliente.FirstOrDefault();
            recibos.Observaciones = "";
            recibos.ComprobanteId = comp;
            return View(recibos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReciboAnular(Comprobantes recibo)
        {
            recibo.UsuarioAlta = User.Identity.Name;
            await comprobantesRepository.spComprobantesReciboAnular(recibo.Id,recibo.Observaciones,recibo.UsuarioAlta);

            return RedirectToAction(nameof(ComprobanteDetalle), new { id = recibo.ComprobanteId });            
        }

    }

}
