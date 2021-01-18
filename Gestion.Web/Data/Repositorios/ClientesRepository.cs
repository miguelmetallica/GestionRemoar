using Gestion.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public class ClientesRepository : GenericRepository<Clientes>, IClientesRepository
    {
        private readonly DataContext context;
        private readonly IFactoryConnection factoryConnection;

        public ClientesRepository(DataContext context, IFactoryConnection factoryConnection) : base(context)
        {
            this.context = context;
            this.factoryConnection = factoryConnection;
        }
        public async Task<bool> ExistCuitCuilAsync(string id,string cuitcuil)
        {
            return await this.context.Clientes.AnyAsync(e => e.CuilCuit == cuitcuil && e.Id != id);
        }

        public async Task<bool> ExistNroDocAsync(string id, string tipo, string nro)
        {
            return await this.context.Clientes.AnyAsync(e => e.TipoDocumentoId == tipo && e.NroDocumento == nro && e.Id != id);
        }

        public Clientes GetOne(string id)
        {
            var cliente = this.context.Clientes.Where(x => x.Id == id).FirstOrDefault();
            return cliente;
        }

        public List<Clientes> GetAllActivos() 
        {
            var clientes = this.context.Clientes.Where(x => x.Estado == true).OrderBy(x => x.NroDocumento).ToList();
            return clientes;
        }

        public async Task<int> spInsertar(ClientesFisicoAdd item)
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
                        oCmd.CommandText = "ClientesPersonaFisicaInsertar";

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
                        oCmd.Parameters.AddWithValue("@TipoResponsableId", item.TipoResponsableId);
                        oCmd.Parameters.AddWithValue("@CategoriaId", item.CategoriaId);
                        oCmd.Parameters.AddWithValue("@Estado", true);
                        oCmd.Parameters.AddWithValue("@Usuario", item.UsuarioAlta);

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
                        oCmd.CommandText = "ClientesPersonaFisicaEditar";

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
                        oCmd.Parameters.AddWithValue("@TipoResponsableId", item.TipoResponsableId);
                        oCmd.Parameters.AddWithValue("@CategoriaId", item.CategoriaId);
                        oCmd.Parameters.AddWithValue("@Estado", true);
                        oCmd.Parameters.AddWithValue("@Usuario", item.UsuarioAlta);

                        //Ejecutamos el comando y retornamos el id generado
                        await oCmd.ExecuteScalarAsync();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el registro: " + ex.Message);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
        }

        public async Task<ClientesDTO> spCliente(string id)
        {
            //Creamos la conexión a utilizar.
            //Utilizamos la sentencia Using para asegurarnos de cerrar la conexión
            //y liberar el objeto al salir de esta sección de manera automática            
            using (var oCnn = factoryConnection.GetConnection())
            {
                using (SqlCommand oCmd = new SqlCommand())
                {
                    //asignamos la conexion de trabajo
                    oCmd.Connection = oCnn;

                    //utilizamos stored procedures
                    oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //el indicamos cual stored procedure utilizar
                    oCmd.CommandText = "ClientesGetUno";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var obj = new ClientesDTO();

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                        {
                            if(oReader.Read())
                            {
                                //si existe algun valor, creamos el objeto y lo almacenamos
                                //en la colección
                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;
                                obj.Apellido = oReader["Apellido"] as string;
                                obj.Nombre = oReader["Nombre"] as string;
                                obj.RazonSocial = oReader["RazonSocial"] as string;

                                obj.TipoDocumentoId = oReader["TipoDocumentoId"] as string;
                                obj.TipoDocumento = oReader["TipoDocumento"] as string;
                                obj.NroDocumento = oReader["NroDocumento"] as string;
                                obj.CuilCuit = oReader["CuilCuit"] as string;

                                if (!DBNull.Value.Equals(oReader["FechaNacimiento"]))
                                    obj.FechaNacimiento = (DateTime)oReader["FechaNacimiento"];
                                obj.esPersonaJuridica = (bool)oReader["esPersonaJuridica"];
                                obj.ProvinciaId = oReader["ProvinciaId"] as string;
                                obj.Provincia = oReader["Provincia"] as string;
                                obj.Localidad = oReader["Localidad"] as string;
                                obj.CodigoPostal = oReader["CodigoPostal"] as string;
                                obj.Calle = oReader["Calle"] as string;
                                obj.CalleNro = oReader["CalleNro"] as string;
                                obj.PisoDpto = oReader["PisoDpto"] as string;
                                obj.OtrasReferencias = oReader["OtrasReferencias"] as string;
                                obj.Telefono = oReader["Telefono"] as string;
                                obj.Celular = oReader["Celular"] as string;
                                obj.Email = oReader["Email"] as string;

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;
                                
                                obj.CategoriaId = oReader["CategoriaId"] as string;
                                obj.Categoria = oReader["Categoria"] as string;
                                                                
                                obj.Estado = (bool)oReader["Estado"];                                                               
                            }
                        }
                        //retornamos los valores encontrados


                        return obj;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos 
                        obj = null;
                    }
                }
            }
        }
    }
}
