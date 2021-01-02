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

        public async Task<Productos> GetProducto(string id)
        {
            return await this.context.Productos
                .Include(o => o.TipoProducto)
                .Include(o => o.CuentaCompra)
                .Include(o => o.CuentaVenta)
                .Include(o => o.UnidadMedida)
                .Include(o => o.Alicuota)
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistCodigoAsync(string id, string codigo)
        {
            return await this.context.Productos.AnyAsync(e => e.Codigo == codigo && e.Id != id);
        }

        public async Task<int> spInsertar(Productos productos)
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

        public async Task<int> spEditar(Productos productos)
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
    }
}
