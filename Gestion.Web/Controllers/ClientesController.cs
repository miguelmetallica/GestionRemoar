using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
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

        public ClientesController(IClientesRepository repository, IUserHelper userHelper, 
                ITiposDocumentosRepository tiposDocumentos, IProvinciasRepository provincias,
                IFactoryConnection factoryConnection,IComprobantesRepository comprobantesRepository,
                IFormasPagosRepository formasPagosRepository)
        {
            this.repository = repository;
            this.userHelper = userHelper;
            this.tiposDocumentos = tiposDocumentos;
            this.provincias = provincias;
            this.factoryConnection = factoryConnection;
            this.comprobantesRepository = comprobantesRepository;
            this.formasPagosRepository = formasPagosRepository;
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

            ViewData["CC"] = await comprobantesRepository.spComprobantes(id);

            return View(cliente.FirstOrDefault());            
        }

        public IActionResult Create()
        {
            ViewBag.TiposDocumentos = this.tiposDocumentos.GetCombo();
            ViewBag.Provincias = this.provincias.GetCombo();
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

                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.TiposDocumentos = this.tiposDocumentos.GetCombo();
            ViewBag.Provincias = this.provincias.GetCombo();

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

                return RedirectToAction(nameof(Index));
            }

            ViewBag.TiposDocumentos = this.tiposDocumentos.GetCombo();
            ViewBag.Provincias = this.provincias.GetCombo();

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
                Email = clientes.Email
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
                Email = clientes.Email
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
                Email = clientes.Email
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

            var cliente = await this.repository.spCliente(id);
            if (cliente == null)
            {
                return NotFound();
            }
            
            ViewData["FormasPagosTMP"] = await comprobantesRepository.spComprobantesTmpFormasPagos(id);
            ViewData["Saldo"] = await comprobantesRepository.spComprobantes(id);
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagos"] = formasPagosRepository.GetAll();
            
            var recibo = new ComprobantesReciboDTO();
            recibo.ClienteId = cliente.FirstOrDefault().Id;
            
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
            
            var formasPagos = await comprobantesRepository.spComprobantesTmpFormasPagos(reciboDTO.ClienteId);

            var SaldoDto = await comprobantesRepository.spComprobantes(reciboDTO.ClienteId);

            if (SaldoDto.Sum(x => x.Saldo) < 0)
            {
                ModelState.AddModelError("Importe", "El Saldo de la Cuenta es Cero");

                ViewData["Saldo"] = SaldoDto;
                ViewData["Cliente"] = cliente.FirstOrDefault();

            }

            if (ModelState.IsValid)
            {
                if (SaldoDto.Sum(x => x.Saldo) < reciboDTO.Importe)
                {
                    ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo de la Cuenta");

                    ViewData["Saldo"] = SaldoDto;
                    ViewData["Cliente"] = cliente.FirstOrDefault();                    

                    return View(reciboDTO);
                }

                reciboDTO.Usuario = User.Identity.Name; 
                await comprobantesRepository.spRecibo(reciboDTO);

                return RedirectToAction(nameof(Details),new { id = reciboDTO.ClienteId });
            }
            
            ViewData["Saldo"] = SaldoDto;            
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagos"] = formasPagosRepository.GetAll();

            return View(reciboDTO);
        }

        public async Task<IActionResult> Efectivo(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            var cliente = await this.repository.spCliente(id);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Saldo"] = await comprobantesRepository.spComprobantes(id);
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagosTMP"] = await comprobantesRepository.spComprobantesTmpFormasPagos(id);

            var recibo = new ComprobantesEfectivoDTO();
            recibo.ClienteId = cliente.FirstOrDefault().Id;
            recibo.FormaPagoId = fp;
            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Efectivo(ComprobantesEfectivoDTO reciboDTO)
        {
            var cliente = await this.repository.spCliente(reciboDTO.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }
            
            var SaldoDto = await comprobantesRepository.spComprobantes(reciboDTO.ClienteId);
            var SaldoTmp = await comprobantesRepository.spComprobantesTmpFormasPagos(reciboDTO.ClienteId);
            if (SaldoDto.Sum(x => x.Saldo) - SaldoTmp.Sum(x => x.Total) == 0)
            {
                ModelState.AddModelError("Importe", "El Saldo de la Cuenta es Cero");
                
                ViewData["Saldo"] = SaldoDto;
                ViewData["Cliente"] = cliente.FirstOrDefault();
                ViewData["FormasPagosTMP"] = SaldoTmp;
            }

            if (ModelState.IsValid)
            {
                if (SaldoDto.Sum(x => x.Saldo) - SaldoTmp.Sum(x => x.Total) < reciboDTO.Importe)
                {
                    ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo de la Cuenta");

                    ViewData["Saldo"] = SaldoDto;
                    ViewData["Cliente"] = cliente.FirstOrDefault();
                    ViewData["FormasPagosTMP"] = SaldoTmp;

                    return View(reciboDTO);
                }

                reciboDTO.Usuario = User.Identity.Name;
                await comprobantesRepository.spEfectivo(reciboDTO);

                return RedirectToAction(nameof(Recibo), new { id = reciboDTO.ClienteId });
            }

            //ViewData["Saldo"] = SaldoDto;
            //ViewData["Cliente"] = cliente.FirstOrDefault();
            //ViewData["FormasPagosTMP"] = await comprobantesRepository.spComprobantesTmpFormasPagos(reciboDTO.ClienteId);

            return View(reciboDTO);
        }

        public async Task<IActionResult> Otro(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            var cliente = await this.repository.spCliente(id);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Saldo"] = await comprobantesRepository.spComprobantes(id);
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagosTMP"] = await comprobantesRepository.spComprobantesTmpFormasPagos(id);

            var recibo = new ComprobantesOtroDTO();
            recibo.ClienteId = cliente.FirstOrDefault().Id;
            recibo.FormaPagoId = fp;
            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Otro(ComprobantesOtroDTO reciboDTO)
        {
            var cliente = await this.repository.spCliente(reciboDTO.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            var SaldoDto = await comprobantesRepository.spComprobantes(reciboDTO.ClienteId);
            var SaldoTmp = await comprobantesRepository.spComprobantesTmpFormasPagos(reciboDTO.ClienteId);
            
            if (SaldoDto.Sum(x => x.Saldo) - SaldoTmp.Sum(x => x.Total) == 0)
            {
                ModelState.AddModelError("Importe", "El Saldo de la Cuenta es Cero");
                
                ViewData["Saldo"] = SaldoDto;
                ViewData["Cliente"] = cliente.FirstOrDefault();
                ViewData["FormasPagosTMP"] = SaldoTmp;
            }

            if (ModelState.IsValid)
            {
                if (SaldoDto.Sum(x => x.Saldo) - SaldoTmp.Sum(x => x.Total) < reciboDTO.Importe)
                {
                    ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo de la Cuenta");

                    ViewData["Saldo"] = SaldoDto;
                    ViewData["Cliente"] = cliente.FirstOrDefault();
                    ViewData["FormasPagosTMP"] = SaldoTmp;

                    return View(reciboDTO);
                }

                reciboDTO.Usuario = User.Identity.Name;
                await comprobantesRepository.spOtro(reciboDTO);

                return RedirectToAction(nameof(Recibo), new { id = reciboDTO.ClienteId });
            }

            //ViewData["Saldo"] = SaldoDto;
            //ViewData["Cliente"] = cliente.FirstOrDefault();

            return View(reciboDTO);
        }

        public async Task<IActionResult> TarjetaDebito(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            var cliente = await this.repository.spCliente(id);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Saldo"] = await comprobantesRepository.spComprobantes(id);
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagosTMP"] = await comprobantesRepository.spComprobantesTmpFormasPagos(id);

            var recibo = new ComprobantesTarjetaDTO();
            recibo.ClienteId = cliente.FirstOrDefault().Id;
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

            
            var cliente = await this.repository.spCliente(reciboDTO.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            var SaldoDto = await comprobantesRepository.spComprobantes(reciboDTO.ClienteId);
            var SaldoTmp = await comprobantesRepository.spComprobantesTmpFormasPagos(reciboDTO.ClienteId);

            if (SaldoDto.Sum(x => x.Saldo) - SaldoTmp.Sum(x => x.Total) == 0)
            {
                ModelState.AddModelError("Importe", "El Saldo de la Cuenta es Cero");
                ViewData["Saldo"] = SaldoDto;
                ViewData["Cliente"] = cliente.FirstOrDefault();
                ViewData["FormasPagosTMP"] = SaldoTmp;

            }
            
            

            if (ModelState.IsValid)
            {
                if (SaldoDto.Sum(x => x.Saldo) - SaldoTmp.Sum(x => x.Total) < reciboDTO.Importe)
                {
                    ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo de la Cuenta");

                    ViewData["Saldo"] = SaldoDto;
                    ViewData["Cliente"] = cliente.FirstOrDefault();
                    ViewData["FormasPagosTMP"] = SaldoTmp;

                    return View(reciboDTO);
                }

                reciboDTO.Usuario = User.Identity.Name;
                reciboDTO.TarjetaEsDebito = true;
                reciboDTO.Cuota = 1;
                reciboDTO.Interes= 0;
                reciboDTO.Total = reciboDTO.Importe;

                await comprobantesRepository.spTarjeta(reciboDTO);

                return RedirectToAction(nameof(Recibo), new { id = reciboDTO.ClienteId });
            }

            ViewData["Saldo"] = SaldoDto;
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagosTMP"] = SaldoTmp;

            return View(reciboDTO);
        }

        public async Task<IActionResult> TarjetaCredito(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            var cliente = await this.repository.spCliente(id);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Saldo"] = await comprobantesRepository.spComprobantes(id);
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagosTMP"] = await comprobantesRepository.spComprobantesTmpFormasPagos(id);

            var recibo = new ComprobantesTarjetaDTO();
            recibo.ClienteId = cliente.FirstOrDefault().Id;
            recibo.FormaPagoId = fp;
            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TarjetaCredito(ComprobantesTarjetaDTO reciboDTO)
        {
            var cliente = await this.repository.spCliente(reciboDTO.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            var SaldoDto = await comprobantesRepository.spComprobantes(reciboDTO.ClienteId);
            var SaldoTmp = await comprobantesRepository.spComprobantesTmpFormasPagos(reciboDTO.ClienteId);

            if (SaldoDto.Sum(x => x.Saldo) - SaldoTmp.Sum(x => x.Total) == 0)
            {
                ModelState.AddModelError("Importe", "El Saldo de la Cuenta es Cero");
                
                ViewData["Saldo"] = SaldoDto;
                ViewData["Cliente"] = cliente.FirstOrDefault();
                ViewData["FormasPagosTMP"] = SaldoTmp;

            }

            if (ModelState.IsValid)
            {
                if (SaldoDto.Sum(x => x.Saldo) - SaldoTmp.Sum(x => x.Total) < reciboDTO.Importe)
                {
                    ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo de la Cuenta");

                    ViewData["Saldo"] = SaldoDto;
                    ViewData["Cliente"] = cliente.FirstOrDefault();
                    ViewData["FormasPagosTMP"] = SaldoTmp;

                    return View(reciboDTO);
                }

                reciboDTO.Usuario = User.Identity.Name;
                reciboDTO.TarjetaEsDebito = false;                                

                await comprobantesRepository.spTarjeta(reciboDTO);

                return RedirectToAction(nameof(Recibo), new { id = reciboDTO.ClienteId });
            }

            //ViewData["Saldo"] = SaldoDto;
            //ViewData["Cliente"] = cliente.FirstOrDefault();
            //ViewData["FormasPagosTMP"] = SaldoTmp;
            
            return View(reciboDTO);
        }

        public async Task<IActionResult> Cheque(string id, string fp)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (fp == null)
            {
                return NotFound();
            }

            var cliente = await this.repository.spCliente(id);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["Saldo"] = await comprobantesRepository.spComprobantes(id);
            ViewData["Cliente"] = cliente.FirstOrDefault();
            ViewData["FormasPagosTMP"] = await comprobantesRepository.spComprobantesTmpFormasPagos(id);

            var recibo = new ComprobantesChequeDTO();
            recibo.ClienteId = cliente.FirstOrDefault().Id;
            recibo.FormaPagoId = fp;
            return View(recibo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cheque(ComprobantesChequeDTO reciboDTO)
        {
            var cliente = await this.repository.spCliente(reciboDTO.ClienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            var SaldoDto = await comprobantesRepository.spComprobantes(reciboDTO.ClienteId);
            var SaldoTmp = await comprobantesRepository.spComprobantesTmpFormasPagos(reciboDTO.ClienteId);

            if (SaldoDto.Sum(x => x.Saldo) - SaldoTmp.Sum(x => x.Total) == 0)
            {
                ModelState.AddModelError("Importe", "El Saldo de la Cuenta es Cero");

                ViewData["Saldo"] = SaldoDto;
                ViewData["Cliente"] = cliente.FirstOrDefault();
                ViewData["FormasPagosTMP"] = SaldoTmp;
            }

            if (ModelState.IsValid)
            {
                if (SaldoDto.Sum(x => x.Saldo) - SaldoTmp.Sum(x => x.Total) < reciboDTO.Importe)
                {
                    ModelState.AddModelError("Importe", "El Importe ingresado es mayor al Saldo de la Cuenta");

                    ViewData["Saldo"] = SaldoDto;
                    ViewData["Cliente"] = cliente.FirstOrDefault();
                    ViewData["FormasPagosTMP"] = SaldoTmp;

                    return View(reciboDTO);
                }

                reciboDTO.Usuario = User.Identity.Name;
                await comprobantesRepository.spCheque(reciboDTO);

                return RedirectToAction(nameof(Recibo), new { id = reciboDTO.ClienteId });
            }

            //ViewData["Saldo"] = SaldoDto;
            //ViewData["Cliente"] = cliente.FirstOrDefault();

            return View(reciboDTO);
        }

        public async Task<IActionResult> DeleteFormaPago(string id,string cli)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (cli == null)
            {
                return NotFound();
            }

            await comprobantesRepository.spDeleteFormaPago(id);
            return RedirectToAction(nameof(Recibo) , new { id = cli });
        }

        public async Task<IActionResult> ComprobanteDetalle(string id, string comp)
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

            ViewData["CC"] = await comprobantesRepository.spComprobantes(id);

            return View(cliente.FirstOrDefault());
        }

    }

}
