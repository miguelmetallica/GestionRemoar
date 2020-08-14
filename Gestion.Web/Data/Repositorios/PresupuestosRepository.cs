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

        public PresupuestosRepository(DataContext context, IUserHelper userHelper, IFactoryConnection factoryConnection) : base(context)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.factoryConnection = factoryConnection;
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

        public PresupuestosDetalle GetDetalle(string id)
        {
            return this.context.PresupuestosDetalle
                .Include(o => o.Producto)
                .Where(o => o.Id == id)
                .FirstOrDefault();
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
                    Precio = (decimal)product.PrecioNormal,
                    Cantidad = model.Cantidad,
                    UsuarioAlta = userName,
                };

                this.context.PresupuestosDetalle.Add(presupuestoDetalle);
            }
            else
            {
                presupuestoDetalle.Cantidad += model.Cantidad;
                this.context.PresupuestosDetalle.Update(presupuestoDetalle);
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

        public Presupuestos GetPresupuestoId(string id)
        {
            return this.context.Presupuestos.Where(x => x.Id == id)
                .Include(x => x.Cliente)
                .Include(x => x.Estado)
                .Include(x => x.Item)
                .ThenInclude(i => i.Producto)
                .FirstOrDefault();                ;
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
                        oCmd.Parameters.AddWithValue("@EstadoId", presupuestos.EstadoId);
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
                        oCmd.Parameters.AddWithValue("@EstadoId", presupuestos.EstadoId);
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

    }
}
