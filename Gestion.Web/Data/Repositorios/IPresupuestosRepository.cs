using Gestion.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IPresupuestosRepository : IGenericRepository<Presupuestos>
    {
        Task<IQueryable<Presupuestos>> GetOrdersAsync(string userName);

        PresupuestosDetalle GetDetalle(string id);

        Task AddItemAsync(PresupuestosDetalle model, string userName);

        Task ModifyCantidadesAsync(string id, int cantidad);

        Task DeleteDetailAsync(string id);

        Task<bool> ConfirmOrderAsync(string userName);

        Task DeliverOrder(DeliverViewModel model);

        Presupuestos GetPresupuestoId(string id);

        Task<int> spInsertar(Presupuestos presupuestos);

        Task<int> spEditar(Presupuestos presupuestos);

    }
}
