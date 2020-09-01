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
    public class ComprobantesRepository : GenericRepository<Comprobantes>, IComprobantesRepository
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private readonly IFactoryConnection factoryConnection;
        private readonly IClientesRepository clientesRepository;
        private readonly IPresupuestosEstadosRepository presupuestosEstados;

        public ComprobantesRepository(DataContext context, 
                                    IUserHelper userHelper, 
                                    IFactoryConnection factoryConnection,
                                    IClientesRepository clientesRepository,
                                    IPresupuestosEstadosRepository presupuestosEstados) : base(context)
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

            var product = await this.context.Productos.FindAsync(model.ProductoId);
            if (product == null)
            {
                return;
            }

            var presupuestoDetalle = await this.context.PresupuestosDetalle
                .Where(odt => odt.PresupuestoId == model.PresupuestoId && odt.ProductoId == model.ProductoId)
                .FirstOrDefaultAsync();
            if (presupuestoDetalle == null)
            {
                presupuestoDetalle = new PresupuestosDetalle
                {
                    Id = Guid.NewGuid().ToString(),
                    PresupuestoId = model.PresupuestoId,
                    ProductoId = model.ProductoId,
                    Precio = (decimal)product.PrecioVenta,
                    Cantidad = model.Cantidad,
                    UsuarioAlta = userName,
                };

                await spInsertarProducto(presupuestoDetalle);
                //this.context.PresupuestosDetalle.Add(presupuestoDetalle);
            }
            else
            {
                presupuestoDetalle.Cantidad += model.Cantidad;
                await spEditarProducto(presupuestoDetalle);
                //this.context.PresupuestosDetalle.Update(presupuestoDetalle);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task ModifyCantidadesAsync(string id, int cantidad)
        {
            var presupuestosDetalle = await this.context.PresupuestosDetalle.FindAsync(id);
            if (presupuestosDetalle == null)
            {
                return;
            }

            presupuestosDetalle.Cantidad += cantidad;
            if (presupuestosDetalle.Cantidad > 0)
            {
                this.context.PresupuestosDetalle.Update(presupuestosDetalle);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task DeleteDetailAsync(string id)
        {
            var presupuestosDetalle = await this.context.PresupuestosDetalle.FindAsync(id);
            if (presupuestosDetalle == null)
            {
                return;
            }

            this.context.PresupuestosDetalle.Remove(presupuestosDetalle);
            await this.context.SaveChangesAsync();
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

        public async Task<int> spAprobar(Presupuestos presupuestos)
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

        public async Task<int> spRechazar(Presupuestos presupuestos)
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

        public async Task<List<PresupuestosDetalleDTO>> spPresupuestosVencido(string id)
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
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = (string)oReader["UsuarioAlta"];

                                obj.ClienteId = (string)oReader["ClienteId"];
                                obj.ClienteCodigo = (string)oReader["ClienteCodigo"];
                                obj.ClienteRazonSocial = (string)oReader["RazonSocial"];
                                obj.ClienteNroDocumento = (string)oReader["NroDocumento"];
                                obj.ClienteCuilCuit = (string)oReader["CuilCuit"];

                                obj.ProductoId = (string)oReader["ProductoId"];
                                obj.ProductoCodigo = (string)oReader["ProductoCodigo"];
                                obj.ProductoNombre = (string)oReader["Producto"];
                                obj.ProductoPrecio = (decimal)oReader["Precio"];
                                obj.ProductoPrecioSinImpuesto = (decimal)oReader["PrecioSinImpuesto"];
                                obj.ProductoCantidad = (int)oReader["Cantidad"];

                                obj.SubTotal = (decimal)oReader["SubTotal"];
                                obj.SubTotalSinImpuesto = (decimal)oReader["SubTotalSinImpuesto"];
                                obj.DetalleId = (string)oReader["DetalleId"];

                                obj.Cantidad = (int)oReader["CantidadTotal"];
                                obj.Total = (decimal)oReader["PrecioTotal"];
                                obj.TotalSinImpuesto = (decimal)oReader["PrecioTotalSinImpuesto"];
                                obj.TotalDescuento = (decimal)oReader["PrecioTotalDescuento"];
                                obj.TotalSinImpuestoDescuento = (decimal)oReader["PrecioTotalSinImpuestoDescuento"];
                                obj.Descuento = (decimal)oReader["DescuentoPorcentaje"];


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

        public async Task<List<PresupuestosDetalleDTO>> spPresupuestosPendiente(string id)
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
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = (string)oReader["UsuarioAlta"];

                                obj.ClienteId = (string)oReader["ClienteId"];
                                obj.ClienteCodigo = (string)oReader["ClienteCodigo"];
                                obj.ClienteRazonSocial = (string)oReader["RazonSocial"];
                                obj.ClienteNroDocumento = (string)oReader["NroDocumento"];
                                obj.ClienteCuilCuit = (string)oReader["CuilCuit"];
                                
                                obj.ProductoId = (string)oReader["ProductoId"];
                                obj.ProductoCodigo = (string)oReader["ProductoCodigo"];
                                obj.ProductoNombre = (string)oReader["Producto"];
                                obj.ProductoPrecio = (decimal)oReader["Precio"];
                                obj.ProductoPrecioSinImpuesto = (decimal)oReader["PrecioSinImpuesto"];
                                obj.ProductoCantidad = (int)oReader["Cantidad"];

                                obj.SubTotal = (decimal)oReader["SubTotal"];
                                obj.SubTotalSinImpuesto = (decimal)oReader["SubTotalSinImpuesto"];
                                obj.DetalleId = (string)oReader["DetalleId"];

                                obj.Cantidad = (int)oReader["CantidadTotal"];
                                obj.Total = (decimal)oReader["PrecioTotal"];
                                obj.TotalSinImpuesto = (decimal)oReader["PrecioTotalSinImpuesto"];
                                obj.TotalDescuento = (decimal)oReader["PrecioTotalDescuento"];
                                obj.TotalSinImpuestoDescuento = (decimal)oReader["PrecioTotalSinImpuestoDescuento"];
                                obj.Descuento = (decimal)oReader["DescuentoPorcentaje"];

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

        public async Task<List<PresupuestosDetalleDTO>> spPresupuestosRechazado(string id)
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
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = (string)oReader["UsuarioAlta"];

                                obj.ClienteId = (string)oReader["ClienteId"];
                                obj.ClienteCodigo = (string)oReader["ClienteCodigo"];
                                obj.ClienteRazonSocial = (string)oReader["RazonSocial"];
                                obj.ClienteNroDocumento = (string)oReader["NroDocumento"];
                                obj.ClienteCuilCuit = (string)oReader["CuilCuit"];

                                obj.ProductoId = (string)oReader["ProductoId"];
                                obj.ProductoCodigo = (string)oReader["ProductoCodigo"];
                                obj.ProductoNombre = (string)oReader["Producto"];
                                obj.ProductoPrecio = (decimal)oReader["Precio"];
                                obj.ProductoPrecioSinImpuesto = (decimal)oReader["PrecioSinImpuesto"];
                                obj.ProductoCantidad = (int)oReader["Cantidad"];

                                obj.SubTotal = (decimal)oReader["SubTotal"];
                                obj.SubTotalSinImpuesto = (decimal)oReader["SubTotalSinImpuesto"];
                                obj.DetalleId = (string)oReader["DetalleId"];

                                obj.Cantidad = (int)oReader["CantidadTotal"];
                                obj.Total = (decimal)oReader["PrecioTotal"];
                                obj.TotalSinImpuesto = (decimal)oReader["PrecioTotalSinImpuesto"];
                                obj.TotalDescuento = (decimal)oReader["PrecioTotalDescuento"];
                                obj.TotalSinImpuestoDescuento = (decimal)oReader["PrecioTotalSinImpuestoDescuento"];
                                obj.Descuento = (decimal)oReader["DescuentoPorcentaje"];


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
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Fecha = (DateTime)oReader["Fecha"];
                                obj.FechaVencimiento = (DateTime)oReader["FechaVencimiento"];
                                obj.Estado = oReader["Estado"].ToString();
                                obj.UsuarioAlta = (string)oReader["UsuarioAlta"];

                                obj.ClienteId = (string)oReader["ClienteId"];
                                obj.ClienteCodigo = (string)oReader["ClienteCodigo"];
                                obj.ClienteRazonSocial = (string)oReader["RazonSocial"];
                                obj.ClienteNroDocumento = (string)oReader["NroDocumento"];
                                obj.ClienteCuilCuit = (string)oReader["CuilCuit"];

                                obj.ProductoId = (string)oReader["ProductoId"];
                                obj.ProductoCodigo = (string)oReader["ProductoCodigo"];
                                obj.ProductoNombre = (string)oReader["Producto"];
                                obj.ProductoPrecio = (decimal)oReader["Precio"];
                                obj.ProductoPrecioSinImpuesto = (decimal)oReader["PrecioSinImpuesto"];
                                obj.ProductoCantidad = (int)oReader["Cantidad"];

                                obj.SubTotal = (decimal)oReader["SubTotal"];
                                obj.SubTotalSinImpuesto = (decimal)oReader["SubTotalSinImpuesto"];
                                obj.DetalleId = (string)oReader["DetalleId"];

                                obj.Cantidad = (int)oReader["CantidadTotal"];
                                obj.Total = (decimal)oReader["PrecioTotal"];
                                obj.TotalSinImpuesto = (decimal)oReader["PrecioTotalSinImpuesto"];
                                obj.TotalDescuento = (decimal)oReader["PrecioTotalDescuento"];
                                obj.TotalSinImpuestoDescuento = (decimal)oReader["PrecioTotalSinImpuestoDescuento"];
                                obj.Descuento = (decimal)oReader["DescuentoPorcentaje"];


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
    }
}
