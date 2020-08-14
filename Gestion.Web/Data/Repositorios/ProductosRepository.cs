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
                        oCmd.Parameters.AddWithValue("@Producto", productos.Producto);
                        oCmd.Parameters.AddWithValue("@TipoProductoId", productos.TipoProductoId);
                        oCmd.Parameters.AddWithValue("@DescripcionCorta", productos.DescripcionCorta);
                        oCmd.Parameters.AddWithValue("@DescripcionLarga", productos.DescripcionLarga);
                        oCmd.Parameters.AddWithValue("@Peso", productos.Peso);
                        oCmd.Parameters.AddWithValue("@DimencionesLongitud", productos.DimencionesLongitud);
                        oCmd.Parameters.AddWithValue("@DimencionesAncho", productos.DimencionesAncho);
                        oCmd.Parameters.AddWithValue("@DimencionesAltura", productos.DimencionesAltura);
                        oCmd.Parameters.AddWithValue("@PrecioCompra", productos.PrecioCompra);
                        oCmd.Parameters.AddWithValue("@CuentaVentaId", productos.CuentaVentaId);
                        oCmd.Parameters.AddWithValue("@CuentaCompraId", productos.CuentaCompraId);
                        oCmd.Parameters.AddWithValue("@UnidadMedidaId", productos.UnidadMedidaId);
                        oCmd.Parameters.AddWithValue("@MarcaId", productos.MarcaId);
                        oCmd.Parameters.AddWithValue("@Visible", productos.Visible);
                        oCmd.Parameters.AddWithValue("@PrecioNormal", productos.PrecioNormal);
                        oCmd.Parameters.AddWithValue("@PrecioRebajado", productos.PrecioRebajado);
                        oCmd.Parameters.AddWithValue("@EstadoInventarioId", null);
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

    }
}
