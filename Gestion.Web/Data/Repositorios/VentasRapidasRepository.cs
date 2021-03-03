using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestion.Web.Data
{
    public class VentasRapidasRepository : GenericRepository<VentasRapidas>, IVentasRapidasRepository
    {

        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private readonly IFactoryConnection factoryConnection;
        private readonly IClientesRepository clientesRepository;

        public VentasRapidasRepository(DataContext context,
                                    IUserHelper userHelper,
                                    IFactoryConnection factoryConnection,
                                    IClientesRepository clientesRepository
            ) : base(context)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.factoryConnection = factoryConnection;
            this.clientesRepository = clientesRepository;

        }

        public async Task<VentasRapidasDTO> spVentasRapidas(string id)
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
                    oCmd.CommandText = "VentasRapidasGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", id);



                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                        {
                            if (oReader.Read())
                            {
                                var obj = new VentasRapidasDTO();

                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.ClienteRazonSocial = oReader["RazonSocial"] as string;
                                obj.ClienteNroDocumento = oReader["NroDocumento"] as string;
                                obj.ClienteCuitCuil = oReader["CuilCuit"] as string;

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteCategoriaId = oReader["ClienteCategoriaId"] as string;
                                obj.ClienteCategoria = oReader["ClienteCategoria"] as string;

                                obj.EstadoId = oReader["EstadoId"] as string;
                                obj.Estado = oReader["Estado"] as string;

                                obj.Precio = (decimal)oReader["TotalProductos"];
                                obj.CantidadProductos = (int)oReader["CantidadProductos"];
                                obj.Saldo = (decimal)oReader["Saldo"];

                                if (!DBNull.Value.Equals(oReader["DescuentoPorcentaje"]))
                                    obj.DescuentoPorcentaje = (decimal)oReader["DescuentoPorcentaje"];

                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                                if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                    obj.FechaEdit = (DateTime)oReader["FechaEdit"];

                                return obj;
                            }
                        }
                        //retornamos los valores encontrados

                        return null;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos                         
                    }
                }
            }
        }

        public async Task<VentasRapidasDTO> spVentasRapidas(string id, string formaPagoId)
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
                    oCmd.CommandText = "[VentasRapidasContadoGet]";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", id);
                    oCmd.Parameters.AddWithValue("@FormaPagoId", formaPagoId);

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                        {
                            if (oReader.Read())
                            {
                                var obj = new VentasRapidasDTO();

                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.ClienteRazonSocial = oReader["RazonSocial"] as string;
                                obj.ClienteNroDocumento = oReader["NroDocumento"] as string;
                                obj.ClienteCuitCuil = oReader["CuilCuit"] as string;

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteCategoriaId = oReader["ClienteCategoriaId"] as string;
                                obj.ClienteCategoria = oReader["ClienteCategoria"] as string;

                                obj.EstadoId = oReader["EstadoId"] as string;
                                obj.Estado = oReader["Estado"] as string;

                                obj.Precio = (decimal)oReader["TotalProductos"];
                                obj.CantidadProductos = (int)oReader["CantidadProductos"];
                                obj.PrecioSinDescuento = (decimal)oReader["TotalProductosSinDescuento"];

                                if (!DBNull.Value.Equals(oReader["DescuentoPorcentaje"]))
                                    obj.DescuentoPorcentaje = (decimal)oReader["DescuentoPorcentaje"];

                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                                if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                    obj.FechaEdit = (DateTime)oReader["FechaEdit"];

                                return obj;
                            }
                        }
                        //retornamos los valores encontrados

                        return null;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos                         
                    }
                }
            }
        }
        public async Task<VentasRapidasDTO> spVentasRapidasFacturadas(string id)
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
                    oCmd.CommandText = "VentasRapidasGetFacturado";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", id);



                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                        {
                            if (oReader.Read())
                            {
                                var obj = new VentasRapidasDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.ClienteRazonSocial = oReader["RazonSocial"] as string;
                                obj.ClienteNroDocumento = oReader["NroDocumento"] as string;
                                obj.ClienteCuitCuil = oReader["CuilCuit"] as string;

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteCategoriaId = oReader["ClienteCategoriaId"] as string;
                                obj.ClienteCategoria = oReader["ClienteCategoria"] as string;

                                obj.EstadoId = oReader["EstadoId"] as string;
                                obj.Estado = oReader["Estado"] as string;

                                obj.Precio = (decimal)oReader["TotalProductos"];
                                obj.CantidadProductos = (int)oReader["CantidadProductos"];
                                obj.Saldo = (decimal)oReader["Saldo"];

                                if (!DBNull.Value.Equals(oReader["DescuentoPorcentaje"]))
                                    obj.DescuentoPorcentaje = (decimal)oReader["DescuentoPorcentaje"];

                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                                if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                    obj.FechaEdit = (DateTime)oReader["FechaEdit"];

                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                obj.TipoComprobante = oReader["TipoComprobante"] as string;
                                obj.CodigoComprobante = oReader["CodigoComprobante"] as string;

                                obj.TipoComprobanteFiscal = oReader["TipoComprobanteFiscal"] as string;
                                obj.LetraFiscal = oReader["LetraFiscal"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVentaFiscal"]))
                                    obj.PtoVentaFiscal = (int)(oReader["PtoVentaFiscal"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["NumeroFiscal"]))
                                    obj.NumeroFiscal = (decimal)(oReader["NumeroFiscal"] ?? 0);

                                //obj.Estado = oReader["Estado"].ToString();
                                //obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                //obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                //obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                                //if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                //    obj.FechaEdit = (DateTime)oReader["FechaEdit"];

                                return obj;
                            }
                        }
                        //retornamos los valores encontrados

                        return null;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos                         
                    }
                }
            }
        }

        public async Task<List<VentasRapidasIndex>> spVentasRapidas()
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
                    oCmd.CommandText = "VentasRapidasGets";

                    //le asignamos el parámetro para el stored procedure
                    //oCmd.Parameters.AddWithValue("@CuentaVentaId", Id);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<VentasRapidasIndex>();

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                        {
                            while (oReader.Read())
                            {
                                //si existe algun valor, creamos el objeto y lo almacenamos
                                //en la colección
                                var obj = new VentasRapidasIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.RazonSocial = oReader["RazonSocial"].ToString();
                                obj.NroDocumento = oReader["NroDocumento"].ToString();
                                obj.CuilCuit = oReader["CuilCuit"].ToString();
                                //obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"].ToString();
                                obj.Total = (decimal)oReader["Precio"];
                                //obj.TotalContado = (decimal)oReader["PrecioContado"];
                                obj.Cantidad = (int)oReader["Cantidad"];

                                //Agregamos el objeto a la coleccion de resultados
                                objs.Add(obj);
                                obj = null;
                            }
                        }
                        //retornamos los valores encontrados
                        return objs;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos 
                        objs = null;
                    }
                }
            }
        }

        public async Task<List<VentasRapidasIndex>> spVentasRapidasFacturadas()
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
                    oCmd.CommandText = "VentasRapidasGetsFacturadas";

                    //le asignamos el parámetro para el stored procedure
                    //oCmd.Parameters.AddWithValue("@CuentaVentaId", Id);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<VentasRapidasIndex>();

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                        {
                            while (oReader.Read())
                            {
                                //si existe algun valor, creamos el objeto y lo almacenamos
                                //en la colección
                                var obj = new VentasRapidasIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.RazonSocial = oReader["RazonSocial"].ToString();
                                obj.NroDocumento = oReader["NroDocumento"].ToString();
                                obj.CuilCuit = oReader["CuilCuit"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"].ToString();
                                obj.Total = (decimal)oReader["Precio"];
                                //obj.TotalContado = (decimal)oReader["PrecioContado"];
                                obj.Cantidad = (int)oReader["Cantidad"];

                                //Agregamos el objeto a la coleccion de resultados
                                objs.Add(obj);
                                obj = null;
                            }
                        }
                        //retornamos los valores encontrados
                        return objs;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos 
                        objs = null;
                    }
                }
            }
        }

        public async Task<int> spInsertar(VentasRapidasDTO VentasRapidasDTO)
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
                        oCmd.CommandText = "VentasRapidasInsertar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", VentasRapidasDTO.Id);
                        oCmd.Parameters.AddWithValue("@ClienteId", VentasRapidasDTO.ClienteId);
                        oCmd.Parameters.AddWithValue("@Usuario", VentasRapidasDTO.UsuarioAlta);

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

        public async Task<List<VentasRapidasDetalleDTO>> spDetalleVentaRapida(string ventaRapidaId)
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
                    oCmd.CommandText = "VentasRapidasGetDetalle";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@VentaRapidaid", ventaRapidaId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<VentasRapidasDetalleDTO>();

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                        {
                            while (oReader.Read())
                            {
                                //si existe algun valor, creamos el objeto y lo almacenamos
                                //en la colección
                                var obj = new VentasRapidasDetalleDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.VentaRapidaId = oReader["VentaRapidaId"] as string;
                                obj.ProductoId = oReader["ProductoId"] as string;
                                obj.ProductoCodigo = oReader["ProductoCodigo"] as string;
                                obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                if (!DBNull.Value.Equals(oReader["Precio"]))
                                    obj.Precio = (decimal)oReader["Precio"];
                                if (!DBNull.Value.Equals(oReader["PrecioSinImpuesto"]))
                                    obj.PrecioSinImpuesto = (decimal)oReader["PrecioSinImpuesto"];
                                if (!DBNull.Value.Equals(oReader["Cantidad"]))
                                    obj.Cantidad = (int)oReader["Cantidad"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                                if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                    obj.FechaEdit = (DateTime)oReader["FechaEdit"];



                                //Agregamos el objeto a la coleccion de resultados
                                objs.Add(obj);
                                obj = null;
                            }
                        }
                        //retornamos los valores encontrados


                        return objs;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos 
                        objs = null;
                    }
                }
            }
        }

        public async Task<VentasRapidasDetalleDTO> spDetalleVentaRapidaId(string Id)
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
                    oCmd.CommandText = "VentasRapidasGetDetalleId";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", Id);

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan

                    //Ejecutamos el comando y retornamos los valores
                    using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                    {
                        while (oReader.Read())
                        {
                            //si existe algun valor, creamos el objeto y lo almacenamos
                            //en la colección
                            var obj = new VentasRapidasDetalleDTO();
                            obj.Id = oReader["Id"] as string;
                            obj.VentaRapidaId = oReader["VentaRapidaId"] as string;
                            obj.ProductoId = oReader["ProductoId"] as string;
                            obj.ProductoCodigo = oReader["ProductoCodigo"] as string;
                            obj.ProductoNombre = oReader["ProductoNombre"] as string;
                            if (!DBNull.Value.Equals(oReader["Precio"]))
                                obj.Precio = (decimal)oReader["Precio"];
                            //if (!DBNull.Value.Equals(oReader["PrecioContado"]))
                            //    obj.PrecioContado = (decimal)oReader["PrecioContado"];
                            if (!DBNull.Value.Equals(oReader["PrecioSinImpuesto"]))
                                obj.PrecioSinImpuesto = (decimal)oReader["PrecioSinImpuesto"];
                            //if (!DBNull.Value.Equals(oReader["PrecioContadoSinImpuesto"]))
                            //    obj.PrecioContadoSinImpuesto = (decimal)oReader["PrecioContadoSinImpuesto"];
                            if (!DBNull.Value.Equals(oReader["Cantidad"]))
                                obj.Cantidad = (int)oReader["Cantidad"];
                            obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                            obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                            obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                            if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                obj.FechaEdit = (DateTime)oReader["FechaEdit"];

                            return obj;
                        }
                    }
                    return null;
                }
            }
        }

        public async Task AddItemAsync(VentasRapidasDetalleDTO model, string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var detalle = await this.context.VentasRapidasDetalle
                .Where(odt => odt.VentaRapidaId == model.VentaRapidaId && odt.ProductoId == model.ProductoId)
                .FirstOrDefaultAsync();
            if (detalle == null)
            {
                detalle = new VentasRapidasDetalle
                {
                    Id = Guid.NewGuid().ToString(),
                    VentaRapidaId = model.VentaRapidaId,
                    ProductoId = model.ProductoId,
                    ProductoCodigo = model.ProductoCodigo,
                    ProductoNombre = model.ProductoNombre,
                    Precio = (decimal)model.Precio,
                    //PrecioContado = (decimal)model.PrecioContado,
                    Cantidad = model.Cantidad,
                    UsuarioAlta = userName,
                };

                await spInsertarProducto(detalle);
            }
            else
            {
                detalle.Precio = model.Precio;
                //detalle.PrecioContado = model.PrecioContado;
                detalle.Cantidad += model.Cantidad;
                await spEditarProducto(detalle);
            }
        }

        public async Task AddItemComodinAsync(VentasRapidasDetalleDTO model, string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var detalle = new VentasRapidasDetalle
            {
                Id = Guid.NewGuid().ToString(),
                VentaRapidaId = model.VentaRapidaId,
                ProductoId = model.ProductoId,
                ProductoCodigo = model.ProductoCodigo,
                ProductoNombre = model.ProductoNombre,
                Precio = (decimal)model.Precio,
                //PrecioContado = (decimal)model.PrecioContado,
                Cantidad = model.Cantidad,
                UsuarioAlta = userName,
            };

            await spInsertarProducto(detalle);
        }

        public async Task<int> spInsertarProducto(VentasRapidasDetalle detalle)
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
                        oCmd.CommandText = "VentaRapidaDetalleInsertar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", detalle.Id);
                        oCmd.Parameters.AddWithValue("@VentaRapidaId", detalle.VentaRapidaId);
                        oCmd.Parameters.AddWithValue("@ProductoId", detalle.ProductoId);
                        oCmd.Parameters.AddWithValue("@ProductoCodigo", detalle.ProductoCodigo);
                        oCmd.Parameters.AddWithValue("@ProductoNombre", detalle.ProductoNombre);
                        oCmd.Parameters.AddWithValue("@Precio", detalle.Precio);
                        //oCmd.Parameters.AddWithValue("@PrecioContado", detalle.PrecioContado);
                        oCmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                        oCmd.Parameters.AddWithValue("@Usuario", detalle.UsuarioAlta);

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

        public async Task<int> spEditarProducto(VentasRapidasDetalle detalle)
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
                        oCmd.CommandText = "VentaRapidaDetalleEditar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", detalle.Id);
                        oCmd.Parameters.AddWithValue("@ProductoId", detalle.ProductoId);
                        oCmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                        oCmd.Parameters.AddWithValue("@Precio", detalle.Precio);
                        oCmd.Parameters.AddWithValue("@Producto", detalle.ProductoNombre);
                        oCmd.Parameters.AddWithValue("@Usuario", detalle.UsuarioAlta);

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

        public async Task DeleteDetailAsync(string id)
        {
            var detalle = await this.context.VentasRapidasDetalle.FindAsync(id);
            if (detalle == null)
            {
                return;
            }

            this.context.VentasRapidasDetalle.Remove(detalle);
            await this.context.SaveChangesAsync();
        }

        public async Task ModifyCantidadesAsync(string id, int cantidad)
        {
            var detalle = await this.context.VentasRapidasDetalle.FindAsync(id);
            if (detalle == null)
            {
                return;
            }

            detalle.Cantidad += cantidad;
            if (detalle.Cantidad > 0)
            {
                this.context.VentasRapidasDetalle.Update(detalle);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<int> spDescuentoAplica(string ventaRapidaId, string descuentoId, string usuario)
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
                        oCmd.CommandText = "VentaRapidaDescuentoAplica";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", ventaRapidaId);
                        oCmd.Parameters.AddWithValue("@DescuentoId", descuentoId);
                        oCmd.Parameters.AddWithValue("@Usuario", usuario);

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

        public async Task<int> spDescuentoBorrar(string Id, string usuario)
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
                        oCmd.CommandText = "VentaRapidaDescuentoBorrar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", Id);
                        oCmd.Parameters.AddWithValue("@Usuario", usuario);

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

        public async Task<int> spClienteEditar(VentasRapidasDTO VentasRapidasDTO)
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
                        oCmd.CommandText = "VentaRapidaDatosCliente";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", VentasRapidasDTO.Id);
                        oCmd.Parameters.AddWithValue("@NroDocumento", VentasRapidasDTO.ClienteNroDocumento);
                        oCmd.Parameters.AddWithValue("@CuilCuit", VentasRapidasDTO.ClienteCuitCuil);
                        oCmd.Parameters.AddWithValue("@RazonSocial", VentasRapidasDTO.ClienteRazonSocial);
                        oCmd.Parameters.AddWithValue("@TipoResponsableId", VentasRapidasDTO.TipoResponsableId);
                        oCmd.Parameters.AddWithValue("@Usuario", VentasRapidasDTO.UsuarioAlta);

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

        public async Task<int> spEfectivo(FormaPagoEfectivoDTO formaPago)
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
                        oCmd.CommandText = "VentaRapidaInsertarEfectivo";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@VentaRapidaId", formaPago.VentaRapidaId);
                        oCmd.Parameters.AddWithValue("@FormaPagoId", formaPago.FormaPagoId);
                        oCmd.Parameters.AddWithValue("@Importe", formaPago.Importe);
                        oCmd.Parameters.AddWithValue("@TotalImporte", formaPago.Saldo);
                        oCmd.Parameters.AddWithValue("@DescuentoImporte", formaPago.SaldoConDescuento);
                        oCmd.Parameters.AddWithValue("@DescuentoPorcentaje", formaPago.Descuento);
                        oCmd.Parameters.AddWithValue("@Observaciones", formaPago.Observaciones);
                        oCmd.Parameters.AddWithValue("@Usuario", formaPago.Usuario);

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
        public async Task<int> spOtro(FormaPagoOtroDTO formaPago)
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
                        oCmd.CommandText = "VentaRapidaInsertarOtro";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@VentaRapidaId", formaPago.VentaRapidaId);
                        oCmd.Parameters.AddWithValue("@FormaPagoId", formaPago.FormaPagoId);
                        oCmd.Parameters.AddWithValue("@Importe", formaPago.Importe);
                        oCmd.Parameters.AddWithValue("@Otros", formaPago.FormaPago);
                        oCmd.Parameters.AddWithValue("@Observaciones", formaPago.Observaciones);
                        oCmd.Parameters.AddWithValue("@CodigoAutorizacion", formaPago.CodigoAutorizacion);
                        oCmd.Parameters.AddWithValue("@Usuario", formaPago.Usuario);

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
        public async Task<int> spTarjeta(FormaPagoTarjetaDTO formaPago)
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
                        oCmd.CommandText = "VentaRapidaInsertarTarjeta";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@VentaRapidaId", formaPago.VentaRapidaId);
                        oCmd.Parameters.AddWithValue("@FormaPagoId", formaPago.FormaPagoId);
                        oCmd.Parameters.AddWithValue("@TarjetaId", formaPago.TarjetaId);
                        oCmd.Parameters.AddWithValue("@Cuota", formaPago.Cuota);
                        oCmd.Parameters.AddWithValue("@Importe", formaPago.Importe);
                        oCmd.Parameters.AddWithValue("@Interes", formaPago.Interes);
                        oCmd.Parameters.AddWithValue("@Total", formaPago.Total);
                        oCmd.Parameters.AddWithValue("@TarjetaCliente", formaPago.TarjetaCliente);
                        oCmd.Parameters.AddWithValue("@TarjetaNumero", formaPago.TarjetaNumero);
                        oCmd.Parameters.AddWithValue("@TarjetaVenceAño", formaPago.TarjetaVenceAño);
                        oCmd.Parameters.AddWithValue("@TarjetaVenceMes", formaPago.TarjetaVenceMes);
                        oCmd.Parameters.AddWithValue("@TarjetaCodigoSeguridad", formaPago.TarjetaCodigoSeguridad);
                        oCmd.Parameters.AddWithValue("@TarjetaEsDebito", formaPago.TarjetaEsDebito);
                        oCmd.Parameters.AddWithValue("@Observaciones", formaPago.Observaciones);
                        oCmd.Parameters.AddWithValue("@CodigoAutorizacion", formaPago.CodigoAutorizacion);
                        oCmd.Parameters.AddWithValue("@Usuario", formaPago.Usuario);

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
        public async Task<int> spCheque(FormaPagoChequeDTO formaPago)
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
                        oCmd.CommandText = "VentaRapidaInsertarCheque";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@VentaRapidaId", formaPago.VentaRapidaId);
                        oCmd.Parameters.AddWithValue("@FormaPagoId", formaPago.FormaPagoId);
                        oCmd.Parameters.AddWithValue("@ChequeBancoId", formaPago.ChequeBancoId);
                        oCmd.Parameters.AddWithValue("@Importe", formaPago.Importe);
                        oCmd.Parameters.AddWithValue("@ChequeNumero", formaPago.ChequeNumero);
                        oCmd.Parameters.AddWithValue("@ChequeFechaEmision", formaPago.ChequeFechaEmision);
                        oCmd.Parameters.AddWithValue("@ChequeFechaVencimiento", formaPago.ChequeFechaVencimiento);
                        oCmd.Parameters.AddWithValue("@ChequeCuit", formaPago.ChequeCuit);
                        oCmd.Parameters.AddWithValue("@ChequeNombre", formaPago.ChequeNombre);
                        oCmd.Parameters.AddWithValue("@ChequeCuenta", formaPago.ChequeCuenta);
                        oCmd.Parameters.AddWithValue("@TotalImporte", formaPago.Saldo);
                        if (formaPago.Descuento > 0)
                        {
                            oCmd.Parameters.AddWithValue("@DescuentoImporte", formaPago.SaldoConDescuento);
                            oCmd.Parameters.AddWithValue("@RecargoImporte", 0);
                        }
                        if (formaPago.Recargo > 0)
                        {
                            oCmd.Parameters.AddWithValue("@DescuentoImporte", 0);
                            oCmd.Parameters.AddWithValue("@RecargoImporte", formaPago.SaldoConDescuento);
                        }

                        if (formaPago.Descuento == 0 && formaPago.Recargo == 0)
                        {
                            oCmd.Parameters.AddWithValue("@DescuentoImporte", 0);
                            oCmd.Parameters.AddWithValue("@RecargoImporte", 0);
                        }
                        oCmd.Parameters.AddWithValue("@Observaciones", formaPago.Observaciones);
                        oCmd.Parameters.AddWithValue("@Usuario", formaPago.Usuario);

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
        public async Task<int> spDolar(FormaPagoDolarDTO formaPago)
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
                        oCmd.CommandText = "VentaRapidaInsertarDolar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@VentaRapidaId", formaPago.VentaRapidaId);
                        oCmd.Parameters.AddWithValue("@FormaPagoId", formaPago.FormaPagoId);
                        oCmd.Parameters.AddWithValue("@Importe", formaPago.Importe);
                        oCmd.Parameters.AddWithValue("@DolarImporte", formaPago.DolarImporte);
                        oCmd.Parameters.AddWithValue("@DolarCotizacion", formaPago.DolarCotizacion);
                        oCmd.Parameters.AddWithValue("@TotalImporte", formaPago.Saldo);
                        oCmd.Parameters.AddWithValue("@DescuentoImporte", formaPago.SaldoConDescuento);
                        oCmd.Parameters.AddWithValue("@Observaciones", formaPago.Observaciones);
                        oCmd.Parameters.AddWithValue("@Usuario", formaPago.Usuario);

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
        public async Task<List<VentasRapidasFormasPagosDTO>> spFormasPagos(string VentaRapidaId)
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
                    oCmd.CommandText = "VentaRapidaFormasPagosGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@VentaRapidaId", VentaRapidaId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<VentasRapidasFormasPagosDTO>();

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                        {
                            while (oReader.Read())
                            {
                                //si existe algun valor, creamos el objeto y lo almacenamos
                                //en la colección
                                var obj = new VentasRapidasFormasPagosDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.VentaRapidaId = oReader["VentaRapidaId"] as string;
                                obj.FormaPagoId = oReader["FormaPagoId"] as string;
                                obj.FormaPagoCodigo = oReader["FormaPagoCodigo"] as string;
                                obj.FormaPagoTipo = oReader["FormaPagoTipo"] as string;
                                obj.FormaPago = oReader["FormaPago"] as string;
                                obj.Importe = (decimal)oReader["Importe"];
                                obj.Cuota = (int)(oReader["Cuota"] ?? 0);
                                obj.Interes = (decimal)oReader["Interes"];
                                obj.Descuento = (decimal)oReader["Descuento"];
                                obj.Total = (decimal)oReader["Total"];
                                obj.TarjetaId = oReader["TarjetaId"] as string;
                                obj.TarjetaNombre = oReader["TarjetaNombre"] as string;
                                obj.TarjetaCliente = oReader["TarjetaCliente"] as string;
                                obj.TarjetaNumero = oReader["TarjetaNumero"] as string;

                                if (!DBNull.Value.Equals(oReader["TarjetaVenceMes"]))
                                    obj.TarjetaVenceMes = (int)(oReader["TarjetaVenceMes"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["TarjetaVenceAño"]))
                                    obj.TarjetaVenceAño = (int)(oReader["TarjetaVenceAño"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["TarjetaCodigoSeguridad"]))
                                    obj.TarjetaCodigoSeguridad = (int)(oReader["TarjetaCodigoSeguridad"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["TarjetaEsDebito"]))
                                    obj.TarjetaEsDebito = (bool)(oReader["TarjetaEsDebito"]);

                                obj.ChequeBancoId = oReader["ChequeBancoId"] as string;
                                obj.ChequeBanco = oReader["ChequeBanco"] as string;
                                obj.ChequeNumero = oReader["ChequeNumero"] as string;

                                if (!DBNull.Value.Equals(oReader["ChequeFechaEmision"]))
                                    obj.ChequeFechaEmision = (DateTime)(oReader["ChequeFechaEmision"] ?? DateTime.Now);

                                if (!DBNull.Value.Equals(oReader["ChequeFechaEmision"]))
                                    obj.ChequeFechaVencimiento = (DateTime)(oReader["ChequeFechaEmision"] ?? DateTime.Now);

                                obj.ChequeCuit = oReader["ChequeCuit"] as string;
                                obj.ChequeNombre = oReader["ChequeNombre"] as string;
                                obj.ChequeCuenta = oReader["ChequeCuenta"] as string;
                                obj.Otros = oReader["Otros"] as string;
                                obj.Observaciones = oReader["Observaciones"] as string;
                                obj.CodigoAutorizacion = oReader["CodigoAutorizacion"] as string;

                                if (!DBNull.Value.Equals(oReader["DolarCotizacion"]))
                                    obj.DolarCotizacion = (decimal)(oReader["DolarCotizacion"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["DolarImporte"]))
                                    obj.DolarImporte = (decimal)(oReader["DolarImporte"] ?? 0);


                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;

                                //Agregamos el objeto a la coleccion de resultados
                                objs.Add(obj);
                                obj = null;
                            }
                        }
                        //retornamos los valores encontrados


                        return objs;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos 
                        objs = null;
                    }
                }
            }
        }

        public async Task<int> spGenerarVentaRapida(VentasRapidasDTO ventas)
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
                        oCmd.CommandText = "ComprobantesInsertarVentaRapida";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@VentaRapidaId", ventas.Id);
                        oCmd.Parameters.AddWithValue("@Usuario", ventas.UsuarioAlta);

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

        public async Task<int> spGenerarCobrazaVentaRapida(VentasRapidasDTO ventas)
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
                        oCmd.CommandText = "VentaRapidaCobranzaInsert";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@VentaRapidaId", ventas.Id);
                        oCmd.Parameters.AddWithValue("@Usuario", ventas.UsuarioAlta);

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

        public async Task<int> spDelete(string ventaRapidaId)
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
                        oCmd.CommandText = "VentaRapidaDelete";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", ventaRapidaId);

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
        public async Task<int> spDeleteFormaPago(string id)
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
                        oCmd.CommandText = "VentaRapidaDeleteFormaPago";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@id", id);

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

        public async Task<VentasRapidasDTO> spVentasRapidasACobrar(string id)
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
                    oCmd.CommandText = "VentasRapidasACobrarGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", id);



                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                        {
                            if (oReader.Read())
                            {
                                var obj = new VentasRapidasDTO();

                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.ClienteRazonSocial = oReader["RazonSocial"] as string;
                                obj.ClienteNroDocumento = oReader["NroDocumento"] as string;
                                obj.ClienteCuitCuil = oReader["CuilCuit"] as string;

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteCategoriaId = oReader["ClienteCategoriaId"] as string;
                                obj.ClienteCategoria = oReader["ClienteCategoria"] as string;

                                obj.EstadoId = oReader["EstadoId"] as string;
                                obj.Estado = oReader["Estado"] as string;

                                obj.Precio = (decimal)oReader["TotalProductos"];
                                obj.CantidadProductos = (int)oReader["CantidadProductos"];
                                obj.Saldo = (decimal)oReader["Saldo"];

                                if (!DBNull.Value.Equals(oReader["DescuentoPorcentaje"]))
                                    obj.DescuentoPorcentaje = (decimal)oReader["DescuentoPorcentaje"];

                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                                if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                    obj.FechaEdit = (DateTime)oReader["FechaEdit"];

                                return obj;
                            }
                        }
                        //retornamos los valores encontrados

                        return null;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos                         
                    }
                }
            }
        }
        public async Task<List<VentasRapidasIndex>> spVentasRapidasACobrar()
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
                    oCmd.CommandText = "VentasRapidasACobrarGets";

                    //le asignamos el parámetro para el stored procedure
                    //oCmd.Parameters.AddWithValue("@CuentaVentaId", Id);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<VentasRapidasIndex>();

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                        {
                            while (oReader.Read())
                            {
                                //si existe algun valor, creamos el objeto y lo almacenamos
                                //en la colección
                                var obj = new VentasRapidasIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.RazonSocial = oReader["RazonSocial"].ToString();
                                obj.NroDocumento = oReader["NroDocumento"].ToString();
                                obj.CuilCuit = oReader["CuilCuit"].ToString();
                                //obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"].ToString();
                                obj.Total = (decimal)oReader["Precio"];
                                //obj.TotalContado = (decimal)oReader["PrecioContado"];
                                obj.Cantidad = (int)oReader["Cantidad"];

                                //Agregamos el objeto a la coleccion de resultados
                                objs.Add(obj);
                                obj = null;
                            }
                        }
                        //retornamos los valores encontrados
                        return objs;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos 
                        objs = null;
                    }
                }
            }
        }

        public async Task<VentasRapidasResumen> spResumenVentaRapida(string ventaRapidaId)
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
                    oCmd.CommandText = "VentasRapidasResumenGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", ventaRapidaId);

                    var obj = new VentasRapidasResumen();
                    //Ejecutamos el comando y retornamos los valores
                    using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                    {
                        if (oReader.Read())
                        {
                            obj.Id = oReader["Id"].ToString();

                            if (!DBNull.Value.Equals(oReader["CantidadProductos"]))

                                obj.CantidadProductos = (int)oReader["CantidadProductos"];

                            if (!DBNull.Value.Equals(oReader["SubTotalProductos"]))
                                obj.SubTotalProductos = (decimal)oReader["SubTotalProductos"];

                            if (!DBNull.Value.Equals(oReader["DescuentoPorcentaje"]))
                                obj.DescuentoPorcentaje = (decimal)oReader["DescuentoPorcentaje"];

                            if (!DBNull.Value.Equals(oReader["DescuentoMonto"]))
                                obj.DescuentoMonto = (decimal)oReader["DescuentoMonto"];

                            if (!DBNull.Value.Equals(oReader["TotalAPagar"]))
                                obj.TotalAPagar = (decimal)oReader["TotalAPagar"];

                            if (!DBNull.Value.Equals(oReader["SaldoAPagar"]))
                                obj.SaldoAPagar = (decimal)oReader["SaldoAPagar"];
                            if (!DBNull.Value.Equals(oReader["SaldoContadoAPagar"]))
                                obj.SaldoContadoAPagar = (decimal)oReader["SaldoContadoAPagar"];

                            return obj;
                        }
                    }
                    return null;

                }
            }
        }

        public VentasRapidasDTO spVentasRapidasFacturadasPrint(string id)
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
                    oCmd.CommandText = "VentasRapidasGetFacturado";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", id);



                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                var obj = new VentasRapidasDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.Codigo = oReader["Codigo"] as string;
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];

                                obj.ClienteId = oReader["ClienteId"] as string;
                                obj.ClienteCodigo = oReader["ClienteCodigo"] as string;
                                obj.ClienteRazonSocial = oReader["RazonSocial"] as string;
                                obj.ClienteNroDocumento = oReader["NroDocumento"] as string;
                                obj.ClienteCuitCuil = oReader["CuilCuit"] as string;

                                obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                obj.ClienteCategoriaId = oReader["ClienteCategoriaId"] as string;
                                obj.ClienteCategoria = oReader["ClienteCategoria"] as string;

                                obj.EstadoId = oReader["EstadoId"] as string;
                                obj.Estado = oReader["Estado"] as string;

                                obj.Precio = (decimal)oReader["TotalProductos"];
                                obj.CantidadProductos = (int)oReader["CantidadProductos"];
                                obj.Saldo = (decimal)oReader["Saldo"];

                                if (!DBNull.Value.Equals(oReader["DescuentoPorcentaje"]))
                                    obj.DescuentoPorcentaje = (decimal)oReader["DescuentoPorcentaje"];

                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                                if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                    obj.FechaEdit = (DateTime)oReader["FechaEdit"];

                                obj.ComprobanteId = oReader["ComprobanteId"] as string;
                                obj.TipoComprobante = oReader["TipoComprobante"] as string;
                                obj.CodigoComprobante = oReader["CodigoComprobante"] as string;

                                obj.TipoComprobanteFiscal = oReader["TipoComprobanteFiscal"] as string;
                                obj.LetraFiscal = oReader["LetraFiscal"] as string;
                                if (!DBNull.Value.Equals(oReader["PtoVentaFiscal"]))
                                    obj.PtoVentaFiscal = (int)(oReader["PtoVentaFiscal"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["NumeroFiscal"]))
                                    obj.NumeroFiscal = (decimal)(oReader["NumeroFiscal"] ?? 0);

                                //obj.Estado = oReader["Estado"].ToString();
                                //obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                //obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                //obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                                //if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                //    obj.FechaEdit = (DateTime)oReader["FechaEdit"];

                                return obj;
                            }
                        }
                        //retornamos los valores encontrados

                        return null;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos                         
                    }
                }
            }
        }

        public List<VentasRapidasDetalleDTO> spDetalleVentaRapidaPrint(string ventaRapidaId)
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
                    oCmd.CommandText = "VentasRapidasGetDetalle";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@VentaRapidaid", ventaRapidaId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<VentasRapidasDetalleDTO>();

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                //si existe algun valor, creamos el objeto y lo almacenamos
                                //en la colección
                                var obj = new VentasRapidasDetalleDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.VentaRapidaId = oReader["VentaRapidaId"] as string;
                                obj.ProductoId = oReader["ProductoId"] as string;
                                obj.ProductoCodigo = oReader["ProductoCodigo"] as string;
                                obj.ProductoNombre = oReader["ProductoNombre"] as string;
                                if (!DBNull.Value.Equals(oReader["Precio"]))
                                    obj.Precio = (decimal)oReader["Precio"];
                                if (!DBNull.Value.Equals(oReader["PrecioSinImpuesto"]))
                                    obj.PrecioSinImpuesto = (decimal)oReader["PrecioSinImpuesto"];
                                if (!DBNull.Value.Equals(oReader["Cantidad"]))
                                    obj.Cantidad = (int)oReader["Cantidad"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                                if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                    obj.FechaEdit = (DateTime)oReader["FechaEdit"];



                                //Agregamos el objeto a la coleccion de resultados
                                objs.Add(obj);
                                obj = null;
                            }
                        }
                        //retornamos los valores encontrados


                        return objs;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos 
                        objs = null;
                    }
                }
            }
        }

        public List<VentasRapidasFormasPagosDTO> spFormasPagosPrint(string VentaRapidaId)
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
                    oCmd.CommandText = "VentaRapidaFormasPagosGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@VentaRapidaId", VentaRapidaId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<VentasRapidasFormasPagosDTO>();

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                //si existe algun valor, creamos el objeto y lo almacenamos
                                //en la colección
                                var obj = new VentasRapidasFormasPagosDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.VentaRapidaId = oReader["VentaRapidaId"] as string;
                                obj.FormaPagoId = oReader["FormaPagoId"] as string;
                                obj.FormaPagoCodigo = oReader["FormaPagoCodigo"] as string;
                                obj.FormaPagoTipo = oReader["FormaPagoTipo"] as string;
                                obj.FormaPago = oReader["FormaPago"] as string;
                                obj.Importe = (decimal)oReader["Importe"];
                                obj.Cuota = (int)(oReader["Cuota"] ?? 0);
                                obj.Interes = (decimal)oReader["Interes"];
                                obj.Descuento = (decimal)oReader["Descuento"];
                                obj.Total = (decimal)oReader["Total"];
                                obj.TarjetaId = oReader["TarjetaId"] as string;
                                obj.TarjetaNombre = oReader["TarjetaNombre"] as string;
                                obj.TarjetaCliente = oReader["TarjetaCliente"] as string;
                                obj.TarjetaNumero = oReader["TarjetaNumero"] as string;

                                if (!DBNull.Value.Equals(oReader["TarjetaVenceMes"]))
                                    obj.TarjetaVenceMes = (int)(oReader["TarjetaVenceMes"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["TarjetaVenceAño"]))
                                    obj.TarjetaVenceAño = (int)(oReader["TarjetaVenceAño"] ?? 0);
                                if (!DBNull.Value.Equals(oReader["TarjetaCodigoSeguridad"]))
                                    obj.TarjetaCodigoSeguridad = (int)(oReader["TarjetaCodigoSeguridad"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["TarjetaEsDebito"]))
                                    obj.TarjetaEsDebito = (bool)(oReader["TarjetaEsDebito"]);

                                obj.ChequeBancoId = oReader["ChequeBancoId"] as string;
                                obj.ChequeBanco = oReader["ChequeBanco"] as string;
                                obj.ChequeNumero = oReader["ChequeNumero"] as string;

                                if (!DBNull.Value.Equals(oReader["ChequeFechaEmision"]))
                                    obj.ChequeFechaEmision = (DateTime)(oReader["ChequeFechaEmision"] ?? DateTime.Now);

                                if (!DBNull.Value.Equals(oReader["ChequeFechaEmision"]))
                                    obj.ChequeFechaVencimiento = (DateTime)(oReader["ChequeFechaEmision"] ?? DateTime.Now);

                                obj.ChequeCuit = oReader["ChequeCuit"] as string;
                                obj.ChequeNombre = oReader["ChequeNombre"] as string;
                                obj.ChequeCuenta = oReader["ChequeCuenta"] as string;
                                obj.Otros = oReader["Otros"] as string;
                                obj.Observaciones = oReader["Observaciones"] as string;
                                obj.CodigoAutorizacion = oReader["CodigoAutorizacion"] as string;

                                if (!DBNull.Value.Equals(oReader["DolarCotizacion"]))
                                    obj.DolarCotizacion = (decimal)(oReader["DolarCotizacion"] ?? 0);

                                if (!DBNull.Value.Equals(oReader["DolarImporte"]))
                                    obj.DolarImporte = (decimal)(oReader["DolarImporte"] ?? 0);


                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;

                                //Agregamos el objeto a la coleccion de resultados
                                objs.Add(obj);
                                obj = null;
                            }
                        }
                        //retornamos los valores encontrados


                        return objs;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos 
                        objs = null;
                    }
                }
            }
        }

        public VentasRapidasResumen spResumenVentaRapidaPrint(string ventaRapidaId)
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
                    oCmd.CommandText = "VentasRapidasResumenGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", ventaRapidaId);

                    var obj = new VentasRapidasResumen();
                    //Ejecutamos el comando y retornamos los valores
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        if (oReader.Read())
                        {
                            obj.Id = oReader["Id"].ToString();

                            if (!DBNull.Value.Equals(oReader["CantidadProductos"]))

                                obj.CantidadProductos = (int)oReader["CantidadProductos"];

                            if (!DBNull.Value.Equals(oReader["SubTotalProductos"]))
                                obj.SubTotalProductos = (decimal)oReader["SubTotalProductos"];

                            if (!DBNull.Value.Equals(oReader["DescuentoPorcentaje"]))
                                obj.DescuentoPorcentaje = (decimal)oReader["DescuentoPorcentaje"];

                            if (!DBNull.Value.Equals(oReader["DescuentoMonto"]))
                                obj.DescuentoMonto = (decimal)oReader["DescuentoMonto"];

                            if (!DBNull.Value.Equals(oReader["TotalAPagar"]))
                                obj.TotalAPagar = (decimal)oReader["TotalAPagar"];

                            if (!DBNull.Value.Equals(oReader["SaldoAPagar"]))
                                obj.SaldoAPagar = (decimal)oReader["SaldoAPagar"];

                            return obj;
                        }
                    }
                    return null;

                }
            }
        }

        public async Task<List<VentasRapidasIndex>> spVentasRapidasSucursal(string sucursalId)
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
                    oCmd.CommandText = "VentasRapidasSucursalGets";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@SucursalId", sucursalId);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<VentasRapidasIndex>();

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                        {
                            while (oReader.Read())
                            {
                                //si existe algun valor, creamos el objeto y lo almacenamos
                                //en la colección
                                var obj = new VentasRapidasIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.RazonSocial = oReader["RazonSocial"].ToString();
                                obj.NroDocumento = oReader["NroDocumento"].ToString();
                                obj.CuilCuit = oReader["CuilCuit"].ToString();
                                //obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"].ToString();
                                obj.Total = (decimal)oReader["Precio"];
                                //obj.TotalContado = (decimal)oReader["PrecioContado"];
                                obj.Cantidad = (int)oReader["Cantidad"];

                                //Agregamos el objeto a la coleccion de resultados
                                objs.Add(obj);
                                obj = null;
                            }
                        }
                        //retornamos los valores encontrados
                        return objs;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos 
                        objs = null;
                    }
                }
            }
        }

        public async Task<List<VentasRapidasIndex>> spVentasRapidasSucursalACobrar(string sucursalId)
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
                    oCmd.CommandText = "VentasRapidasSucursalACobrarGets";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@sucursalId", sucursalId);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<VentasRapidasIndex>();

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                        {
                            while (oReader.Read())
                            {
                                //si existe algun valor, creamos el objeto y lo almacenamos
                                //en la colección
                                var obj = new VentasRapidasIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.RazonSocial = oReader["RazonSocial"].ToString();
                                obj.NroDocumento = oReader["NroDocumento"].ToString();
                                obj.CuilCuit = oReader["CuilCuit"].ToString();
                                //obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"].ToString();
                                obj.Total = (decimal)oReader["Precio"];
                                //obj.TotalContado = (decimal)oReader["PrecioContado"];
                                obj.Cantidad = (int)oReader["Cantidad"];

                                //Agregamos el objeto a la coleccion de resultados
                                objs.Add(obj);
                                obj = null;
                            }
                        }
                        //retornamos los valores encontrados
                        return objs;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos 
                        objs = null;
                    }
                }
            }
        }

        public async Task<List<VentasRapidasIndex>> spVentasRapidasSucursalFacturadas(string sucursalId)
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
                    oCmd.CommandText = "VentasRapidasSucursalGetsFacturadas";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@sucursalId", sucursalId);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<VentasRapidasIndex>();

                    //No retornamos DataSets, siempre utilizamos objetos para hacernos 
                    //independientes de la estructura de las tablas en el resto
                    //de las capas. Para ellos leemos con el DataReader y creamos
                    //los objetos asociados que se esperan
                    try
                    {
                        //Ejecutamos el comando y retornamos los valores
                        using (SqlDataReader oReader = await oCmd.ExecuteReaderAsync())
                        {
                            while (oReader.Read())
                            {
                                //si existe algun valor, creamos el objeto y lo almacenamos
                                //en la colección
                                var obj = new VentasRapidasIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.RazonSocial = oReader["RazonSocial"].ToString();
                                obj.NroDocumento = oReader["NroDocumento"].ToString();
                                obj.CuilCuit = oReader["CuilCuit"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"].ToString();
                                obj.Total = (decimal)oReader["Precio"];
                                //obj.TotalContado = (decimal)oReader["PrecioContado"];
                                obj.Cantidad = (int)oReader["Cantidad"];

                                //Agregamos el objeto a la coleccion de resultados
                                objs.Add(obj);
                                obj = null;
                            }
                        }
                        //retornamos los valores encontrados
                        return objs;
                    }

                    finally
                    {
                        //el Finally nos da siempre la oportunidad de liberar
                        //la memoria utilizada por los objetos 
                        objs = null;
                    }
                }
            }
        }
    }
}
