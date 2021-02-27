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
    public class PresupuestosRepository : GenericRepository<Presupuestos>, IPresupuestosRepository
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private readonly IFactoryConnection factoryConnection;
        private readonly IClientesRepository clientesRepository;
        private readonly IPresupuestosEstadosRepository presupuestosEstados;

        public PresupuestosRepository(DataContext context, 
                                    IUserHelper userHelper, 
                                    IFactoryConnection factoryConnection,
                                    IClientesRepository clientesRepository,
                                    IPresupuestosEstadosRepository presupuestosEstados
                                    
            ) : base(context)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.factoryConnection = factoryConnection;
            this.clientesRepository = clientesRepository;
            this.presupuestosEstados = presupuestosEstados;
        }

        public async Task<IQueryable<Presupuestos>> GetOrdersAsync(string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            if (await this.userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return this.context.Presupuestos
                    .Include(o => o.Cliente)
                    .Include(o => o.Estado)
                    .Include(o => o.Item)
                    .ThenInclude(i => i.Producto)
                    .OrderByDescending(o => o.Fecha);
            }

            return this.context.Presupuestos
                    .Include(o => o.Cliente)
                    .Include(o => o.Estado)
                    .Include(o => o.Item)
                    .ThenInclude(i => i.Producto)
                    .OrderByDescending(o => o.Fecha);
        }

        public async Task<PresupuestosDetalle> GetDetalle(string id)
        {
            return await this.context.PresupuestosDetalle
                .Include(o => o.Producto)
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddItemAsync(PresupuestosDetalle model, string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            //var presupuestoDetalle = await this.context.PresupuestosDetalle
            //    .Where(odt => odt.PresupuestoId == model.PresupuestoId && odt.ProductoId == model.ProductoId)
            //    .FirstOrDefaultAsync();
            //if (presupuestoDetalle == null)
            //{

            var presupuestoDetalle = new PresupuestosDetalle
            {
                Id = Guid.NewGuid().ToString(),
                PresupuestoId = model.PresupuestoId,
                ProductoId = model.ProductoId,
                ProductoCodigo = model.ProductoCodigo,
                ProductoNombre = model.ProductoNombre,
                Precio = (decimal)model.Precio,
                Cantidad = model.Cantidad,
                UsuarioAlta = userName,
            };

            await spInsertarProducto(presupuestoDetalle);                
            //}
            //else
            //{
            //    presupuestoDetalle.Precio = model.Precio;
            //    presupuestoDetalle.PrecioContado = model.PrecioContado;
            //    presupuestoDetalle.Cantidad += model.Cantidad;
            //    await spEditarProducto(presupuestoDetalle);                
            //}            
        }

        public async Task AddItemComodinAsync(PresupuestosDetalle model, string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var presupuestoDetalle = new PresupuestosDetalle
            {
                Id = Guid.NewGuid().ToString(),
                PresupuestoId = model.PresupuestoId,
                ProductoId = model.ProductoId,
                ProductoCodigo = model.ProductoCodigo,
                ProductoNombre = model.ProductoNombre,
                Precio = (decimal)model.Precio,
                PrecioContado = (decimal)model.PrecioContado,
                Cantidad = model.Cantidad,
                UsuarioAlta = userName,
            };

            await spInsertarProducto(presupuestoDetalle);        
        }               

        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return false;
            }

            var orderTmps = await this.context.PresupuestosDetalleTemp
                .Include(o => o.Producto)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            var details = orderTmps.Select(o => new PresupuestosDetalle
            {
                Id = Guid.NewGuid().ToString(),
                Precio = o.Precio,
                Producto = o.Producto,
                Cantidad = o.Cantidad
            }).ToList();

            var order = new Presupuestos
            {
                Id = Guid.NewGuid().ToString(),
                Fecha = DateTime.Now,
                FechaVencimiento = DateTime.Now.AddDays(3),
                //User = user,
                //Items = details,
            };

            this.context.Presupuestos.Add(order);
            this.context.PresupuestosDetalleTemp.RemoveRange(orderTmps);
            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task DeliverOrder(DeliverViewModel model)
        {
            var order = await this.context.Presupuestos.FindAsync(model.Id);
            if (order == null)
            {
                return;
            }

            this.context.Presupuestos.Update(order);
            await this.context.SaveChangesAsync();
        }


        public async Task<bool> GetProductExist(string presupuestoId, string productoId)
        {
            return await this.context.PresupuestosDetalle.AnyAsync(x => x.PresupuestoId == presupuestoId && x.ProductoId == productoId);
        }

        public Presupuestos GetPresupuestoPendientesId(string id)
        {
            var config = this.context.SistemaConfiguraciones.Where(x => x.Configuracion == "PRESUPUESTO_PENDIENTE_CLIENTE").FirstOrDefault();
            var pendiente = this.context.ParamPresupuestosEstados.Where(x => x.Codigo == config.Valor).FirstOrDefault();

            return this.context.Presupuestos.Where(x => x.Id == id && x.EstadoId == pendiente.Id)
                .Include(x => x.Cliente)
                .Include(x => x.Estado)
                .Include(x => x.Item)
                .ThenInclude(i => i.Producto)
                .FirstOrDefault();
        }

        public Presupuestos GetPresupuestoId(string id)
        {
            return this.context.Presupuestos.Where(x => x.Id == id)
                .Include(x => x.Cliente)
                .Include(x => x.Estado)
                .Include(x => x.Item)
                .ThenInclude(i => i.Producto)
                .FirstOrDefault();
        }

        public async Task<int> spInsertar(Presupuestos presupuestos)
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
                        oCmd.CommandText = "PresupuestosInsertar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", presupuestos.Id);
                        oCmd.Parameters.AddWithValue("@ClienteId", presupuestos.ClienteId);
                        //oCmd.Parameters.AddWithValue("@EstadoId", presupuestos.EstadoId);
                        oCmd.Parameters.AddWithValue("@Usuario", presupuestos.UsuarioAlta);

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

        public async Task<int> spEditar(Presupuestos presupuestos)
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
                        oCmd.CommandText = "PresupuestosEditar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", presupuestos.Id);
                        oCmd.Parameters.AddWithValue("@ClienteId", presupuestos.ClienteId);
                        //oCmd.Parameters.AddWithValue("@EstadoId", presupuestos.EstadoId);
                        oCmd.Parameters.AddWithValue("@Usuario", presupuestos.UsuarioAlta);

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

        public async Task<int> spAprobar(PresupuestosDTO presupuestos)
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
                        oCmd.CommandText = "PresupuestosAprobar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", presupuestos.Id);
                        oCmd.Parameters.AddWithValue("@Usuario", presupuestos.UsuarioAprobacionRechazo);

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

        public async Task<int> spRechazar(PresupuestosDTO presupuestos)
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
                        oCmd.CommandText = "PresupuestosRechazar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", presupuestos.Id);
                        oCmd.Parameters.AddWithValue("@Usuario", presupuestos.UsuarioAprobacionRechazo);

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

        public async Task<int> spInsertarProducto(PresupuestosDetalle presupuestos)
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
                        oCmd.CommandText = "PresupuestosDetalleInsertar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", presupuestos.Id);
                        oCmd.Parameters.AddWithValue("@PresupuestoId", presupuestos.PresupuestoId);
                        oCmd.Parameters.AddWithValue("@ProductoId", presupuestos.ProductoId);
                        oCmd.Parameters.AddWithValue("@ProductoCodigo", presupuestos.ProductoCodigo);
                        oCmd.Parameters.AddWithValue("@ProductoNombre", presupuestos.ProductoNombre);
                        oCmd.Parameters.AddWithValue("@Precio", presupuestos.Precio);
                        oCmd.Parameters.AddWithValue("@Cantidad", presupuestos.Cantidad);
                        oCmd.Parameters.AddWithValue("@Usuario", presupuestos.UsuarioAlta);

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

        public async Task<int> spEditarProducto(PresupuestosDetalle presupuestos)
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
                        oCmd.CommandText = "PresupuestosDetalleEditar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", presupuestos.Id);
                        oCmd.Parameters.AddWithValue("@Cantidad", presupuestos.Cantidad);
                        oCmd.Parameters.AddWithValue("@Precio", presupuestos.Precio);
                        oCmd.Parameters.AddWithValue("@PrecioContado", presupuestos.PrecioContado);
                        oCmd.Parameters.AddWithValue("@Usuario", presupuestos.UsuarioAlta);

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

        public async Task<List<PresupuestosIndex>> spPresupuestosPendientes()
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
                    oCmd.CommandText = "PresupuestosGetPendientes";

                    //le asignamos el parámetro para el stored procedure
                    //oCmd.Parameters.AddWithValue("@CuentaVentaId", Id);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<PresupuestosIndex>();

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
                                var obj = new PresupuestosIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.RazonSocial = oReader["RazonSocial"].ToString();
                                obj.NroDocumento = oReader["NroDocumento"].ToString();
                                obj.CuilCuit = oReader["CuilCuit"].ToString();                                
                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"].ToString();
                                obj.Total = (decimal)oReader["Precio"];
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

        public async Task<List<PresupuestosIndex>> spPresupuestosVencidos()
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
                    oCmd.CommandText = "PresupuestosGetVencidos";

                    //le asignamos el parámetro para el stored procedure
                    //oCmd.Parameters.AddWithValue("@CuentaVentaId", Id);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<PresupuestosIndex>();

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
                                var obj = new PresupuestosIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.RazonSocial = oReader["RazonSocial"].ToString();
                                obj.NroDocumento = oReader["NroDocumento"].ToString();
                                obj.CuilCuit = oReader["CuilCuit"].ToString();
                                obj.Estado = oReader["Estado"].ToString();
                                obj.Total = (decimal)oReader["Precio"];
                                //obj.TotalContado = (decimal)oReader["PrecioContado"];
                                obj.Cantidad = (int)oReader["Cantidad"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"].ToString();

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

        public async Task<PresupuestosDTO> spPresupuestosVencido(string id)
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
                    oCmd.CommandText = "PresupuestosGetVencido";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var obj = new PresupuestosDTO();
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

                                obj.DescuentoId = oReader["DescuentoId"] as string;
                                obj.DescuentoPorcentaje = (decimal)oReader["DescuentoPorcentaje"];

                                if (!DBNull.Value.Equals(oReader["FechaAprobacion"]))
                                    obj.FechaAprobacion = (DateTime)oReader["FechaAprobacion"];
                                if (!DBNull.Value.Equals(oReader["FechaRechazo"]))
                                    obj.FechaRechazo = (DateTime)oReader["FechaRechazo"];

                                obj.MotivoAprobacionRechazo = oReader["MotivoAprobacionRechazo"] as string;
                                obj.UsuarioAprobacionRechazo = oReader["UsuarioAprobacionRechazo"] as string;

                                obj.Precio = (decimal)oReader["Precio"];
                                obj.CantidadProductos = (int)oReader["CantidadProductos"];

                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                                if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                    obj.FechaEdit = (DateTime)oReader["FechaEdit"];

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

        public async Task<PresupuestosDTO> spPresupuestosPendiente(string id)
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
                    oCmd.CommandText = "PresupuestosGetPendiente";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", id);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var obj = new PresupuestosDTO();
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

                                obj.DescuentoId = oReader["DescuentoId"] as string;
                                obj.DescuentoPorcentaje = (decimal)oReader["DescuentoPorcentaje"];

                                if (!DBNull.Value.Equals(oReader["FechaAprobacion"]))
                                    obj.FechaAprobacion = (DateTime)oReader["FechaAprobacion"];
                                if (!DBNull.Value.Equals(oReader["FechaRechazo"]))
                                    obj.FechaRechazo = (DateTime)oReader["FechaRechazo"];

                                obj.MotivoAprobacionRechazo = oReader["MotivoAprobacionRechazo"] as string;
                                obj.UsuarioAprobacionRechazo= oReader["UsuarioAprobacionRechazo"] as string;

                                obj.Precio = (decimal)oReader["Precio"];
                                //obj.PrecioContado = (decimal)oReader["PrecioContado"];
                                obj.CantidadProductos = (int)oReader["CantidadProductos"];
                                
                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                                if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                    obj.FechaEdit = (DateTime)oReader["FechaEdit"];
                                
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

        public async Task<int> spVencidoCopiar(Presupuestos presupuestos, string id)
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
                        oCmd.CommandText = "PresupuestosVencidoCopia";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@VencidoId", presupuestos.Id);
                        oCmd.Parameters.AddWithValue("@Id", id);
                        oCmd.Parameters.AddWithValue("@Usuario", presupuestos.UsuarioAlta);

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

        public async Task<int> spPresupuestoCopiar(PresupuestosDTO presupuestos, string id)
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
                        oCmd.CommandText = "PresupuestosCopia";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@OldId", presupuestos.Id);
                        oCmd.Parameters.AddWithValue("@Id", id);
                        oCmd.Parameters.AddWithValue("@Usuario", presupuestos.UsuarioAlta);

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

        public async Task<List<PresupuestosIndex>> spPresupuestosRechazados()
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
                    oCmd.CommandText = "PresupuestosGetRechazados";

                    //le asignamos el parámetro para el stored procedure
                    //oCmd.Parameters.AddWithValue("@CuentaVentaId", Id);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<PresupuestosIndex>();

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
                                var obj = new PresupuestosIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.RazonSocial = oReader["RazonSocial"].ToString();
                                obj.NroDocumento = oReader["NroDocumento"].ToString();
                                obj.CuilCuit = oReader["CuilCuit"].ToString();
                                obj.Estado = oReader["Estado"].ToString();
                                obj.Total = (decimal)oReader["Precio"];
                                //obj.TotalContado = (decimal)oReader["PrecioContado"];
                                obj.Cantidad = (int)oReader["Cantidad"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"].ToString();

                                if (!DBNull.Value.Equals(oReader["FechaRechazo"]))
                                    obj.FechaRechazo = (DateTime)oReader["FechaRechazo"];

                                obj.UsuarioAprobacionRechazo = oReader["UsuarioAprobacionRechazo"] as string;
                                obj.MotivoAprobacionRechazo = oReader["MotivoAprobacionRechazo"] as string;

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

        public async Task<PresupuestosDTO> spPresupuestosRechazado(string id)
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
                    oCmd.CommandText = "PresupuestosGetRechazado";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var obj = new PresupuestosDTO();
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

                                obj.DescuentoId = oReader["DescuentoId"] as string;
                                obj.DescuentoPorcentaje = (decimal)oReader["DescuentoPorcentaje"];

                                if (!DBNull.Value.Equals(oReader["FechaAprobacion"]))
                                    obj.FechaAprobacion = (DateTime)oReader["FechaAprobacion"];
                                if (!DBNull.Value.Equals(oReader["FechaRechazo"]))
                                    obj.FechaRechazo = (DateTime)oReader["FechaRechazo"];

                                obj.MotivoAprobacionRechazo = oReader["MotivoAprobacionRechazo"] as string;
                                obj.UsuarioAprobacionRechazo = oReader["UsuarioAprobacionRechazo"] as string;

                                obj.Precio = (decimal)oReader["Precio"];
                                obj.CantidadProductos = (int)oReader["CantidadProductos"];

                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                                if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                    obj.FechaEdit = (DateTime)oReader["FechaEdit"];

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

        public async Task<List<PresupuestosIndex>> spPresupuestosAprobados()
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
                    oCmd.CommandText = "PresupuestosGetAprobados";

                    //le asignamos el parámetro para el stored procedure
                    //oCmd.Parameters.AddWithValue("@CuentaVentaId", Id);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<PresupuestosIndex>();

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
                                var obj = new PresupuestosIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.RazonSocial = oReader["RazonSocial"].ToString();
                                obj.NroDocumento = oReader["NroDocumento"].ToString();
                                obj.CuilCuit = oReader["CuilCuit"].ToString();
                                obj.Estado = oReader["Estado"].ToString();
                                obj.Total = (decimal)oReader["Precio"];
                                //obj.TotalContado = (decimal)oReader["PrecioContado"];
                                obj.Cantidad = (int)oReader["Cantidad"];
                                obj.UsuarioAlta = oReader["UsuarioAlta"].ToString();

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

        public async Task<List<PresupuestosDetalleDTO>> spPresupuestosAprobado(string id)
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
                    oCmd.CommandText = "PresupuestosGetAprobado";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<PresupuestosDetalleDTO>();

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
                                var obj = new PresupuestosDetalleDTO();
                                obj.Id = oReader["Id"].ToString();
                                //obj.Codigo = oReader["Codigo"].ToString();
                                //obj.Fecha = (DateTime)oReader["Fecha"];
                                //obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                //obj.Estado = oReader["Estado"].ToString();
                                //obj.UsuarioAlta = (string)oReader["UsuarioAlta"];

                                //obj.ClienteId = (string)oReader["ClienteId"];
                                //obj.ClienteCodigo = (string)oReader["ClienteCodigo"];
                                //obj.ClienteRazonSocial = (string)oReader["RazonSocial"];
                                //obj.ClienteNroDocumento = (string)oReader["NroDocumento"];
                                //obj.ClienteCuilCuit = (string)oReader["CuilCuit"];

                                //obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                //obj.TipoResponsable = oReader["TipoResponsable"] as string ;

                                //obj.ProductoId = (string)oReader["ProductoId"];
                                //obj.ProductoCodigo = (string)oReader["ProductoCodigo"];
                                //obj.ProductoNombre = (string)oReader["Producto"];
                                //obj.ProductoPrecio = (decimal)oReader["Precio"];
                                //obj.ProductoPrecioSinImpuesto = (decimal)oReader["PrecioSinImpuesto"];
                                //obj.ProductoCantidad = (int)oReader["Cantidad"];

                                //obj.SubTotal = (decimal)oReader["SubTotal"];
                                //obj.SubTotalSinImpuesto = (decimal)oReader["SubTotalSinImpuesto"];
                                //obj.DetalleId = (string)oReader["DetalleId"];

                                //obj.Cantidad = (int)oReader["CantidadTotal"];
                                //obj.Total = (decimal)oReader["PrecioTotal"];
                                //obj.TotalSinImpuesto = (decimal)oReader["PrecioTotalSinImpuesto"];
                                //obj.TotalDescuento = (decimal)oReader["PrecioTotalDescuento"];
                                //obj.TotalSinImpuestoDescuento = (decimal)oReader["PrecioTotalSinImpuestoDescuento"];
                                //obj.Descuento = (decimal)oReader["DescuentoPorcentaje"];


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

        public async Task<int> spDescuentoAplica(string presupuestoId, string descuentoId,string usuario)
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
                        oCmd.CommandText = "PresupuestosDescuentoAplica";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", presupuestoId);
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

        public async Task<int> spIncrementa(string Id, int Cantidad, string usuario)
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
                        oCmd.CommandText = "PresupuestosDetalleIncrDecrElim";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", Id);
                        oCmd.Parameters.AddWithValue("@Cantidad", Cantidad);
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

        public async Task<int> spDecrementa(string Id, int Cantidad, string usuario)
        {
            try
            {
                Cantidad = -1 * Cantidad;
                using (var oCnn = factoryConnection.GetConnection())
                {
                    using (SqlCommand oCmd = new SqlCommand())
                    {
                        //asignamos la conexion de trabajo
                        oCmd.Connection = oCnn;

                        //utilizamos stored procedures
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //el indicamos cual stored procedure utilizar
                        oCmd.CommandText = "PresupuestosDetalleIncrDecrElim";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", Id);
                        oCmd.Parameters.AddWithValue("@Cantidad", Cantidad);
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

        public async Task<int> spElimina(string Id, string usuario)
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
                        oCmd.CommandText = "PresupuestosDetalleIncrDecrElim";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", Id);
                        oCmd.Parameters.AddWithValue("@Cantidad", 0);
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

        public async Task<int> spTipoResponsableAplica(string presupuestoId, string tipoResponsableId, string usuario)
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
                        oCmd.CommandText = "PresupuestosTipoResponsableAplica";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", presupuestoId);
                        oCmd.Parameters.AddWithValue("@TipoResponsableId", tipoResponsableId);
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

        public async Task<PresupuestosImprimirDTO> spPresupuestosImprimir(string id)
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
                    oCmd.CommandText = "PresupuestosGetImprimir";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var obj = new PresupuestosImprimirDTO();
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

                                obj.DescuentoId = oReader["DescuentoId"] as string;
                                obj.DescuentoPorcentaje = (decimal)oReader["DescuentoPorcentaje"];

                                if (!DBNull.Value.Equals(oReader["FechaAprobacion"]))
                                    obj.FechaAprobacion = (DateTime)oReader["FechaAprobacion"];
                                if (!DBNull.Value.Equals(oReader["FechaRechazo"]))
                                    obj.FechaRechazo = (DateTime)oReader["FechaRechazo"];

                                obj.MotivoAprobacionRechazo = oReader["MotivoAprobacionRechazo"] as string;
                                obj.UsuarioAprobacionRechazo = oReader["UsuarioAprobacionRechazo"] as string;

                                obj.Precio = (decimal)oReader["Precio"];
                                //obj.PrecioContado = (decimal)oReader["PrecioContado"];
                                obj.CantidadProductos = (int)oReader["CantidadProductos"];

                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                                if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                    obj.FechaEdit = (DateTime)oReader["FechaEdit"];

                                obj.SucursalNombre = oReader["SucursalNombre"] as string;
                                obj.SucursalCalle = oReader["SucursalCalle"] as string;
                                obj.SucursalCalleNro = oReader["SucursalCalleNro"] as string;
                                obj.SucursalLocalidad = oReader["SucursalLocalidad"] as string;
                                obj.SucursalCodigoPostal = oReader["SucursalCodigoPostal"] as string;
                                obj.SucursalTelefono = oReader["SucursalTelefono"] as string;

                                obj.VersionImpresion = oReader["VersionImpresion"] as string;

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
        
        public async Task<PresupuestosDetalleDTO> spPresupuestosDetalle(string id)
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
                    oCmd.CommandText = "PresupuestosDetalleGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<PresupuestosDetalleDTO>();

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
                                var obj = new PresupuestosDetalleDTO();
                                obj.Id = oReader["Id"].ToString();
                                //obj.Codigo = oReader["Codigo"].ToString();
                                //obj.Fecha = (DateTime)oReader["Fecha"];
                                //obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                //obj.Estado = oReader["Estado"].ToString();
                                //obj.UsuarioAlta = (string)oReader["UsuarioAlta"];

                                //obj.ClienteId = (string)oReader["ClienteId"];
                                //obj.ClienteCodigo = (string)oReader["ClienteCodigo"];
                                //obj.ClienteRazonSocial = (string)oReader["RazonSocial"];
                                //obj.ClienteNroDocumento = (string)oReader["NroDocumento"];
                                //obj.ClienteCuilCuit = (string)oReader["CuilCuit"];

                                //obj.TipoResponsableId = oReader["TipoResponsableId"] as string;
                                //obj.TipoResponsable = oReader["TipoResponsable"] as string;

                                //obj.ProductoId = (string)oReader["ProductoId"];
                                //obj.ProductoCodigo = (string)oReader["ProductoCodigo"];
                                //obj.ProductoNombre = (string)oReader["Producto"];
                                //obj.ProductoPrecio = (decimal)oReader["Precio"];
                                //obj.ProductoPrecioSinImpuesto = (decimal)oReader["PrecioSinImpuesto"];
                                //obj.ProductoCantidad = (int)oReader["Cantidad"];

                                //obj.SubTotal = (decimal)oReader["SubTotal"];
                                //obj.SubTotalSinImpuesto = (decimal)oReader["SubTotalSinImpuesto"];
                                //obj.DetalleId = (string)oReader["DetalleId"];

                                //obj.Cantidad = (int)oReader["CantidadTotal"];
                                //obj.Total = (decimal)oReader["PrecioTotal"];
                                //obj.TotalSinImpuesto = (decimal)oReader["PrecioTotalSinImpuesto"];
                                //obj.TotalDescuento = (decimal)oReader["PrecioTotalDescuento"];
                                //obj.TotalSinImpuestoDescuento = (decimal)oReader["PrecioTotalSinImpuestoDescuento"];
                                //obj.Descuento = (decimal)oReader["DescuentoPorcentaje"];


                                //Agregamos el objeto a la coleccion de resultados
                                objs.Add(obj);
                                obj = null;
                            }
                        }
                        //retornamos los valores encontrados


                        return objs.FirstOrDefault();
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

        public async Task<List<PresupuestosDetalleDTO>> spPresupuestosDetallePresupuesto(string presupuestoId)
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
                    oCmd.CommandText = "PresupuestosDetalleGetPresupuesto";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@PresupuestoId", presupuestoId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<PresupuestosDetalleDTO>();

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
                                var obj = new PresupuestosDetalleDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.PresupuestoId = oReader["PresupuestoId"] as string;
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
                                    obj.Cantidad= (int)oReader["Cantidad"];
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

        public async Task<PresupuestosDetalleDTO> spPresupuestosDetalleId(string Id)
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
                    oCmd.CommandText = "PresupuestosDetalleGetId";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", Id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var obj = new PresupuestosDetalleDTO();

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
                                //si existe algun valor, creamos el objeto y lo almacenamos
                                //en la colección
                                obj.Id = oReader["Id"] as string;
                                obj.PresupuestoId = oReader["PresupuestoId"] as string;
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

        public async Task<int> spDescuentoBorrar(string Id,string usuario)
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
                        oCmd.CommandText = "PresupuestosDescuentoBorrar";

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

        public async Task<int> spDatosFiscalesBorrar(string Id, string usuario)
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
                        oCmd.CommandText = "ComprobantesBorrarDatosFiscales";

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

        public async Task<PresupuestosDTO> spPresupuesto(string id)
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
                    oCmd.CommandText = "PresupuestosGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", id);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var obj = new PresupuestosDTO();
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

                                obj.DescuentoId = oReader["DescuentoId"] as string;
                                obj.DescuentoPorcentaje = (decimal)oReader["DescuentoPorcentaje"];

                                if (!DBNull.Value.Equals(oReader["FechaAprobacion"]))
                                    obj.FechaAprobacion = (DateTime)oReader["FechaAprobacion"];
                                if (!DBNull.Value.Equals(oReader["FechaRechazo"]))
                                    obj.FechaRechazo = (DateTime)oReader["FechaRechazo"];

                                obj.MotivoAprobacionRechazo = oReader["MotivoAprobacionRechazo"] as string;
                                obj.UsuarioAprobacionRechazo = oReader["UsuarioAprobacionRechazo"] as string;

                                obj.Precio = (decimal)oReader["Precio"];
                                obj.CantidadProductos = (int)oReader["CantidadProductos"];

                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"] as string;
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioEdit = oReader["UsuarioEdit"] as string;

                                if (!DBNull.Value.Equals(oReader["FechaEdit"]))
                                    obj.FechaEdit = (DateTime)oReader["FechaEdit"];

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

        public async Task<List<PresupuestosIndex>> spPresupuestosRechazados(string ClienteId)
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
                    oCmd.CommandText = "PresupuestosGetRechazadosCliente";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@ClienteId", ClienteId);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<PresupuestosIndex>();

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
                                var obj = new PresupuestosIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.RazonSocial = oReader["RazonSocial"].ToString();
                                obj.NroDocumento = oReader["NroDocumento"].ToString();
                                obj.CuilCuit = oReader["CuilCuit"].ToString();
                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"].ToString();
                                obj.Total = (decimal)oReader["Precio"];
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

        public async Task<List<PresupuestosIndex>> spPresupuestosVencidos(string ClienteId)
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
                    oCmd.CommandText = "PresupuestosGetVencidosCliente";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@ClienteId", ClienteId);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<PresupuestosIndex>();

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
                                var obj = new PresupuestosIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.RazonSocial = oReader["RazonSocial"].ToString();
                                obj.NroDocumento = oReader["NroDocumento"].ToString();
                                obj.CuilCuit = oReader["CuilCuit"].ToString();
                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = oReader["UsuarioAlta"].ToString();
                                obj.Total = (decimal)oReader["Precio"];
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

        public async Task<PresupuestosResumen> spResumenPresupuesto(string presupuestoId)
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
                    oCmd.CommandText = "PresupuestosResumenGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", presupuestoId);

                    var obj = new PresupuestosResumen();
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

                            return obj;
                        }
                    }
                    return null;

                }
            }
        }

        public async Task<List<PresupuestosFormasPagosDTO>> spFormasPagos(string presupuestoId)
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
                    oCmd.CommandText = "PresupuestosFormasPagosGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@PresupuestoId", presupuestoId);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<PresupuestosFormasPagosDTO>();

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
                                var obj = new PresupuestosFormasPagosDTO();
                                obj.Id = oReader["Id"] as string;
                                obj.PresupuestoId = oReader["PresupuestoId"] as string;
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
                        oCmd.CommandText = "PresupuestoInsertarEfectivo";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@PresupuestoId", formaPago.VentaRapidaId);
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
                        oCmd.CommandText = "PresupuestoInsertarOtro";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@PresupuestoId", formaPago.VentaRapidaId);
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
                        oCmd.CommandText = "PresupuestoInsertarTarjeta";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@PresupuestoId", formaPago.VentaRapidaId);
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
                        oCmd.CommandText = "PresupuestoInsertarCheque";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@PresupuestoId", formaPago.VentaRapidaId);
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
                        oCmd.CommandText = "PresupuestoInsertarDolar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@PresupuestoId", formaPago.VentaRapidaId);
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
                        oCmd.CommandText = "PresupuestoDeleteFormaPago";

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
    }
}
