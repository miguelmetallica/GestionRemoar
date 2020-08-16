using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public ClientesController(IClientesRepository repository, IUserHelper userHelper, ITiposDocumentosRepository tiposDocumentos, IProvinciasRepository provincias,IFactoryConnection factoryConnection)
        {
            this.repository = repository;
            this.userHelper = userHelper;
            this.tiposDocumentos = tiposDocumentos;
            this.provincias = provincias;
            this.factoryConnection = factoryConnection;
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

            var cliente = await this.repository.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);            
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
                var cuitcuil = await this.repository.ExistCuitCuilAsync("",clientes.CuilCuit);
                if (cuitcuil)
                {
                    ModelState.AddModelError("CuilCuit", "El Cuil ya esta cargado en la base de datos");                                       
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
            var cuitcuil = await this.repository.ExistCuitCuilAsync(clientes.Id, clientes.CuilCuit);
            if (cuitcuil)
            {
                ModelState.AddModelError("CuilCuit", "El Cuil ya esta cargado en la base de datos");
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

    }

}
