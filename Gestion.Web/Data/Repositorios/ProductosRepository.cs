using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public class ProductosRepository : GenericRepository<Productos>, IProductosRepository
    {
        private readonly DataContext context;
        private readonly IFactoryConnection factoryConnection;

        public ProductosRepository(DataContext context, IFactoryConnection factoryConnection) : base(context)
        {
            this.context = context;
            this.factoryConnection = factoryConnection;
        }

        public IEnumerable<SelectListItem> GetComboProducts()
        {
            var list = this.context.Productos.Select(p => new SelectListItem
            {
                Text = p.Producto,
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select un producto...)",
                Value = ""
            });

            return list;
        }

        public async Task<bool> ExistCodigoAsync(string id, string codigo)
        {
            return await this.context.Productos.AnyAsync(e => e.Codigo == codigo && e.Id != id);
        }

        public async Task<int> spInsertar(ProductosDTO productos)
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
                        oCmd.CommandText = "ProductosInsertar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", productos.Id);
                        oCmd.Parameters.AddWithValue("@TipoProductoId", productos.TipoProductoId);
                        oCmd.Parameters.AddWithValue("@Producto", productos.Producto);                        
                        oCmd.Parameters.AddWithValue("@DescripcionCorta", productos.DescripcionCorta);
                        oCmd.Parameters.AddWithValue("@DescripcionLarga", productos.DescripcionLarga);
                        oCmd.Parameters.AddWithValue("@CodigoBarra", productos.CodigoBarra);
                        oCmd.Parameters.AddWithValue("@Peso", productos.Peso);
                        oCmd.Parameters.AddWithValue("@DimencionesLongitud", productos.DimencionesLongitud);
                        oCmd.Parameters.AddWithValue("@DimencionesAncho", productos.DimencionesAncho);
                        oCmd.Parameters.AddWithValue("@DimencionesAltura", productos.DimencionesAltura);                        
                        oCmd.Parameters.AddWithValue("@CuentaCompraId", productos.CuentaCompraId);
                        oCmd.Parameters.AddWithValue("@CuentaVentaId", productos.CuentaVentaId);
                        oCmd.Parameters.AddWithValue("@UnidadMedidaId", productos.UnidadMedidaId);
                        oCmd.Parameters.AddWithValue("@AlicuotaId", productos.AlicuotaId);
                        oCmd.Parameters.AddWithValue("@PrecioVenta", productos.PrecioVenta);

                        oCmd.Parameters.AddWithValue("@ProveedorId", productos.ProveedorId);
                        oCmd.Parameters.AddWithValue("@CategoriaId", productos.CategoriaId);

                        oCmd.Parameters.AddWithValue("@AceptaDescuento", productos.AceptaDescuento);
                        oCmd.Parameters.AddWithValue("@PrecioRebaja", productos.PrecioRebaja);
                        oCmd.Parameters.AddWithValue("@RebajaDesde", productos.RebajaDesde);
                        oCmd.Parameters.AddWithValue("@RebajaHasta", productos.RebajaHasta);
                        oCmd.Parameters.AddWithValue("@ControlaStock", productos.ControlaStock);

                        oCmd.Parameters.AddWithValue("@Estado", true);
                        oCmd.Parameters.AddWithValue("@EsVendedor", productos.EsVendedor);
                        oCmd.Parameters.AddWithValue("@Usuario", productos.UsuarioAlta);

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

        public async Task<int> spEditar(ProductosDTO productos)
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
                        oCmd.CommandText = "ProductosEditar";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@Id", productos.Id);
                        oCmd.Parameters.AddWithValue("@TipoProductoId", productos.TipoProductoId);
                        oCmd.Parameters.AddWithValue("@Producto", productos.Producto);
                        oCmd.Parameters.AddWithValue("@DescripcionCorta", productos.DescripcionCorta);
                        oCmd.Parameters.AddWithValue("@DescripcionLarga", productos.DescripcionLarga);
                        oCmd.Parameters.AddWithValue("@CodigoBarra", productos.CodigoBarra);
                        oCmd.Parameters.AddWithValue("@Peso", productos.Peso);
                        oCmd.Parameters.AddWithValue("@DimencionesLongitud", productos.DimencionesLongitud);
                        oCmd.Parameters.AddWithValue("@DimencionesAncho", productos.DimencionesAncho);
                        oCmd.Parameters.AddWithValue("@DimencionesAltura", productos.DimencionesAltura);
                        oCmd.Parameters.AddWithValue("@CuentaCompraId", productos.CuentaCompraId);
                        oCmd.Parameters.AddWithValue("@CuentaVentaId", productos.CuentaVentaId);
                        oCmd.Parameters.AddWithValue("@UnidadMedidaId", productos.UnidadMedidaId);
                        oCmd.Parameters.AddWithValue("@AlicuotaId", productos.AlicuotaId);
                        oCmd.Parameters.AddWithValue("@PrecioVenta", productos.PrecioVenta);

                        oCmd.Parameters.AddWithValue("@ProveedorId", productos.ProveedorId);
                        oCmd.Parameters.AddWithValue("@CategoriaId", productos.CategoriaId);

                        oCmd.Parameters.AddWithValue("@AceptaDescuento", productos.AceptaDescuento);
                        oCmd.Parameters.AddWithValue("@PrecioRebaja", productos.PrecioRebaja);
                        oCmd.Parameters.AddWithValue("@RebajaDesde", productos.RebajaDesde);
                        oCmd.Parameters.AddWithValue("@RebajaHasta", productos.RebajaHasta);
                        oCmd.Parameters.AddWithValue("@ControlaStock", productos.ControlaStock);

                        oCmd.Parameters.AddWithValue("@Estado", productos.Estado);
                        oCmd.Parameters.AddWithValue("@Usuario", productos.UsuarioAlta);

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

        public async Task<List<ProductosIndex>> spProductosGet()
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
                    oCmd.CommandText = "ProductosGet";

                    //le asignamos el parámetro para el stored procedure
                    //oCmd.Parameters.AddWithValue("@CuentaVentaId", Id);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ProductosIndex>();

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
                                var obj = new ProductosIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.TipoProducto = (string)oReader["TipoProducto"];
                                obj.Producto = (string)oReader["Producto"];
                                obj.PrecioVenta = (decimal)oReader["PrecioVenta"];
                                obj.PrecioContado = (decimal)oReader["PrecioRebaja"];
                                obj.Estado = (bool)oReader["Estado"];
                                
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

        public async Task<List<ProductosCatalogo>> spCatalogoProductosGet()
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
                    oCmd.CommandText = "CatalogoProductosGet";

                    //le asignamos el parámetro para el stored procedure
                    //oCmd.Parameters.AddWithValue("@CuentaVentaId", Id);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ProductosCatalogo>();

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
                                var obj = new ProductosCatalogo();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.Producto = (string)oReader["Producto"];
                                obj.ImagenUrl = (string)oReader["ImagenUrl"];
                                obj.Cantidad = (int)oReader["Cantidad"];
                                obj.Precio = (decimal)oReader["PrecioVenta"];
                                
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

        public async Task<ProductosDTO> spProductosOne(string Id)
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
                    oCmd.CommandText = "ProductoGet";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", Id);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ProductosDTO>();

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
                                var obj = new ProductosDTO();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.TipoProductoId = (string)oReader["TipoProductoId"];
                                obj.TipoProducto = (string)oReader["TipoProducto"];
                                obj.Producto = (string)oReader["Producto"];
                                obj.DescripcionCorta = (string)oReader["DescripcionCorta"];
                                obj.DescripcionLarga = (string)oReader["DescripcionLarga"];
                                obj.CodigoBarra = (string)oReader["CodigoBarra"];
                                obj.Peso = (decimal)oReader["Peso"];
                                obj.DimencionesLongitud = (decimal)oReader["DimencionesLongitud"];
                                obj.DimencionesAncho = (decimal)oReader["DimencionesAncho"];
                                obj.DimencionesAltura = (decimal)oReader["DimencionesAltura"];
                                obj.CuentaVentaId = (string)oReader["CuentaVentaId"];
                                obj.CuentaVenta = (string)oReader["CuentaVenta"];
                                obj.CuentaCompraId = (string)oReader["CuentaCompraId"];
                                obj.CuentaCompra = (string)oReader["CuentaCompra"];

                                obj.UnidadMedidaId = (string)oReader["UnidadMedidaId"];
                                obj.UnidadMedida = (string)oReader["UnidadMedida"];
                                obj.AlicuotaId = (string)oReader["AlicuotaId"];
                                obj.Alicuota = (string)oReader["Alicuota"];
                                obj.PrecioVenta = (decimal)oReader["PrecioVenta"];
                                obj.ProveedorId = oReader["ProveedorId"] as string;
                                obj.Proveedor = oReader["Proveedor"] as string;
                                obj.CategoriaId = oReader["CategoriaId"] as string;
                                obj.Categoria = oReader["Categoria"] as string;
                                obj.ControlaStock = (bool)oReader["ControlaStock"];
                                obj.AceptaDescuento = (bool)oReader["AceptaDescuento"];

                                if (!DBNull.Value.Equals(oReader["RebajaDesde"]))
                                    obj.RebajaDesde = (DateTime)oReader["RebajaDesde"];
                                else
                                    obj.RebajaDesde = DateTime.Today;

                                if (!DBNull.Value.Equals(oReader["RebajaHasta"]))
                                    obj.RebajaHasta = (DateTime)oReader["RebajaHasta"];
                                else
                                    obj.RebajaHasta = DateTime.Today;

                                obj.PrecioRebaja = (decimal)oReader["PrecioRebaja"];
                                
                                obj.Estado = (bool)oReader["Estado"];
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = (string)oReader["UsuarioAlta"];

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

        public async Task<ProductosDTO> spProductosOneCodigo(string Codigo)
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
                    oCmd.CommandText = "ProductoGetCodigo";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Codigo", Codigo);


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ProductosDTO>();

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
                                var obj = new ProductosDTO();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.TipoProductoId = (string)oReader["TipoProductoId"];
                                obj.TipoProducto = (string)oReader["TipoProducto"];
                                obj.Producto = (string)oReader["Producto"];
                                obj.DescripcionCorta = (string)oReader["DescripcionCorta"];
                                obj.DescripcionLarga = (string)oReader["DescripcionLarga"];
                                obj.CodigoBarra = (string)oReader["CodigoBarra"];
                                obj.Peso = (decimal)oReader["Peso"];
                                obj.DimencionesLongitud = (decimal)oReader["DimencionesLongitud"];
                                obj.DimencionesAncho = (decimal)oReader["DimencionesAncho"];
                                obj.DimencionesAltura = (decimal)oReader["DimencionesAltura"];
                                obj.CuentaVentaId = (string)oReader["CuentaVentaId"];
                                obj.CuentaVenta = (string)oReader["CuentaVenta"];
                                obj.CuentaCompraId = (string)oReader["CuentaCompraId"];
                                obj.CuentaCompra = (string)oReader["CuentaCompra"];

                                obj.UnidadMedidaId = (string)oReader["UnidadMedidaId"];
                                obj.UnidadMedida = (string)oReader["UnidadMedida"];
                                obj.AlicuotaId = (string)oReader["AlicuotaId"];
                                obj.Alicuota = (string)oReader["Alicuota"];
                                obj.PrecioVenta = (decimal)oReader["PrecioVenta"];
                                obj.ProveedorId = oReader["ProveedorId"] as string;
                                obj.Proveedor = oReader["Proveedor"] as string;
                                obj.CategoriaId = oReader["CategoriaId"] as string;
                                obj.Categoria = oReader["Categoria"] as string;
                                obj.ControlaStock = (bool)oReader["ControlaStock"];
                                obj.AceptaDescuento = (bool)oReader["AceptaDescuento"];

                                if (!DBNull.Value.Equals(oReader["RebajaDesde"]))
                                    obj.RebajaDesde = (DateTime)oReader["RebajaDesde"];
                                else
                                    obj.RebajaDesde = DateTime.Today;

                                if (!DBNull.Value.Equals(oReader["RebajaHasta"]))
                                    obj.RebajaHasta = (DateTime)oReader["RebajaHasta"];
                                else
                                    obj.RebajaHasta = DateTime.Today;

                                obj.PrecioRebaja = (decimal)oReader["PrecioRebaja"];

                                obj.Estado = (bool)oReader["Estado"];
                                obj.FechaAlta = (DateTime)oReader["FechaAlta"];
                                obj.UsuarioAlta = (string)oReader["UsuarioAlta"];

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

        public async Task<List<ProductosIndex>> spProductosVentaGet()
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
                    oCmd.CommandText = "ProductosVentaGet";

                    //le asignamos el parámetro para el stored procedure
                    //oCmd.Parameters.AddWithValue("@CuentaVentaId", Id);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ProductosIndex>();

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
                                var obj = new ProductosIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.TipoProducto = (string)oReader["TipoProducto"];
                                obj.Producto = (string)oReader["Producto"];
                                obj.PrecioVenta = (decimal)oReader["PrecioVenta"];
                                obj.PrecioContado = (decimal)oReader["PrecioContado"];
                                obj.Estado = (bool)oReader["Estado"];

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

        public async Task<ProductosIndex> spProductosVentaGetOne(string Id)
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
                    oCmd.CommandText = "ProductosVentaGetOne";

                    //le asignamos el parámetro para el stored procedure
                    oCmd.Parameters.AddWithValue("@Id", Id);                    


                    //aunque debemos buscar solo un elemento, siempre devolvemos
                    //una colección. Es más fácil de manipular y controlar 
                    var objs = new List<ProductosIndex>();

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
                                var obj = new ProductosIndex();
                                obj.Id = oReader["Id"].ToString();
                                obj.Codigo = oReader["Codigo"].ToString();
                                obj.TipoProducto = (string)oReader["TipoProducto"];
                                obj.Producto = (string)oReader["Producto"];
                                obj.PrecioVenta = (decimal)oReader["PrecioVenta"];
                                obj.PrecioContado = (decimal)oReader["PrecioContado"];
                                obj.Estado = (bool)oReader["Estado"];

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
    }
}
