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
        Task AddItemComodinAsync(PresupuestosDetalle model, string userName);

        Task ModifyCantidadesAsync(string id, int cantidad);

        Task DeleteDetailAsync(string id);

        Task<bool> ConfirmOrderAsync(string userName);

        Task DeliverOrder(DeliverViewModel model);

        Presupuestos GetPresupuestoPendientesId(string id);
        Presupuestos GetPresupuestoId(string id);

        Task<int> spInsertar(Presupuestos presupuestos);

        Task<int> spEditar(Presupuestos presupuestos);

        Task<int> spAprobar(PresupuestosDTO presupuestos);
        Task<int> spRechazar(PresupuestosDTO presupuestos);

        Task<List<PresupuestosIndex>> spPresupuestosPendientes();
        Task<PresupuestosDTO> spPresupuestosPendiente(string id);
        Task<List<PresupuestosIndex>> spPresupuestosVencidos();
        Task<PresupuestosDTO> spPresupuestosVencido(string id);

        Task<int> spVencidoCopiar(Presupuestos presupuestos, string id);
        Task<int> spPresupuestoCopiar(PresupuestosDTO presupuestos, string id);

        Task<List<PresupuestosIndex>> spPresupuestosRechazados();

        Task<PresupuestosDTO> spPresupuestosRechazado(string id);

        Task<List<PresupuestosIndex>> spPresupuestosAprobados();

        Task<List<PresupuestosDetalleDTO>> spPresupuestosAprobado(string id);

        Task<int> spDescuentoAplica(string presupuestoId, string descuentoId, string usuario);

        Task<int> spTipoResponsableAplica(string presupuestoId, string tipoResponsableId, string usuario);

        Task<PresupuestosImprimirDTO> spPresupuestosImprimir(string id);

        Task<PresupuestosDetalleDTO> spPresupuestosDetalle(string id);

        Task<List<PresupuestosDetalleDTO>> spPresupuestosDetallePresupuesto(string presupuestoId);
        Task<PresupuestosDetalleDTO> spPresupuestosDetalleId(string Id);
        Task<int> spDescuentoBorrar(string Id, string usuario);
        Task<int> spDatosFiscalesBorrar(string Id, string usuario);

        Task<PresupuestosDTO> spPresupuesto(string id);
    }
}
