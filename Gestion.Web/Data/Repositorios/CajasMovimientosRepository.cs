using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public class CajasMovimientosRepository : GenericRepository<CajasMovimientos>, ICajasMovimientosRepository
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private readonly IFactoryConnection factoryConnection;

        public CajasMovimientosRepository(DataContext context, 
            IUserHelper userHelper,
            IFactoryConnection factoryConnection) : base(context)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.factoryConnection = factoryConnection;
        }

        public async Task<IQueryable<CajasMovimientos>> GetMovimientos(string usuario)
        {
            var user = await userHelper.GetUserByEmailAsync(usuario);
            return this.context.CajasMovimientos.Where(x=> x.SucursalId == user.SucursalId)
                            .Include(x => x.Caja)
                            .Include(x => x.Caja.Sucursal)
                            .Include(x => x.TipoMovimiento);
        }

        public async Task<IQueryable<CajasMovimientos>> GetMovimientosAll(string usuario)
        {
            var user = await userHelper.GetUserByEmailAsync(usuario);
            return this.context.CajasMovimientos
                            .Include(x => x.Caja)
                            .Include(x => x.Caja.Sucursal)
                            .Include(x => x.TipoMovimiento);
        }

        public async Task<int> spPagoProveedorInsert(CajasPagoProveedorDTO item)
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
                        oCmd.CommandText = "CajasMovimientosPagoProveedorInsert";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@SucursalId", item.SucursalId);
                        oCmd.Parameters.AddWithValue("@CajaId", item.CajaId);
                        oCmd.Parameters.AddWithValue("@TipoMovimientoId", item.TipoMovimientoId);
                        oCmd.Parameters.AddWithValue("@ProveedorId", item.ProveedorId);
                        oCmd.Parameters.AddWithValue("@Observaciones", item.Observaciones);
                        oCmd.Parameters.AddWithValue("@NroComprobante", item.NroComprobante);
                        oCmd.Parameters.AddWithValue("@Importe", item.Importe);
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

        public async Task<int> spEnvioDineroSucursalInsert(CajasPagoProveedorDTO item)
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
                        oCmd.CommandText = "CajasMovimientosEnvioDineroSucursalInsert";

                        //le asignamos los parámetros para el stored procedure
                        //los valores viene en el parámetro item del procedimiento
                        oCmd.Parameters.AddWithValue("@SucursalId", item.SucursalId);
                        oCmd.Parameters.AddWithValue("@CajaId", item.CajaId);
                        oCmd.Parameters.AddWithValue("@TipoMovimientoId", item.TipoMovimientoId);
                        oCmd.Parameters.AddWithValue("@ProveedorId", item.ProveedorId);
                        oCmd.Parameters.AddWithValue("@Observaciones", item.Observaciones);
                        oCmd.Parameters.AddWithValue("@NroComprobante", item.NroComprobante);
                        oCmd.Parameters.AddWithValue("@Importe", item.Importe);
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
    }
}
