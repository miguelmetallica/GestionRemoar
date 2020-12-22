using Gestion.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IPresupuestosRepository : IGenericRepository<Presupuestos>
    {
        Task<IQueryable<Presupuestos>> GetOrdersAsync(string userName);

        Task<PresupuestosDetalle> GetDetalle(string id);

        Task AddItemAsync(PresupuestosDetalle model, string userName);

        Task ModifyCantidadesAsync(string id, int cantidad);

        Task DeleteDetailAsync(string id);

        Task<bool> ConfirmOrderAsync(string userName);

        Task DeliverOrder(DeliverViewModel model);

        Presupuestos GetPresupuestoPendientesId(string id);
        Presupuestos GetPresupuestoId(string id);

        Task<int> spInsertar(Presupuestos presupuestos);

        Task<int> spEditar(Presupuestos presupuestos);

        Task<int> spAprobar(Presupuestos presupuestos);
        Task<int> spRechazar(Presupuestos presupuestos);

        Task<List<PresupuestosIndex>> spPresupuestosPendientes();
        Task<List<PresupuestosDetalleDTO>> spPresupuestosPendiente(string id);
        Task<List<PresupuestosIndex>> spPresupuestosVencidos();
        Task<List<PresupuestosDetalleDTO>> spPresupuestosVencido(string id);

        Task<int> spVencidoCopiar(Presupuestos presupuestos, string id);
        Task<int> spPresupuestoCopiar(Presupuestos presupuestos, string id);

        Task<List<PresupuestosIndex>> spPresupuestosRechazados();

        Task<List<PresupuestosDetalleDTO>> spPresupuestosRechazado(string id);

        Task<List<PresupuestosIndex>> spPresupuestosAprobados();

        Task<List<PresupuestosDetalleDTO>> spPresupuestosAprobado(string id);

        Task<int> spDescuentoAplica(string presupuestoId, string descuentoId, string usuario);

        Task<int> spTipoResponsableAplica(string presupuestoId, string tipoResponsableId, string usuario);

        Task<List<PresupuestosDetalleDTO>> spPresupuestosImprimir(string id);

        Task<PresupuestosDetalleDTO> spPresupuestosDetalle(string id);
    }
}
