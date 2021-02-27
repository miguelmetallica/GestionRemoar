using Gestion.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IVentasRapidasRepository : IGenericRepository<VentasRapidas>
    {
        Task<List<VentasRapidasIndex>> spVentasRapidas();
        Task<VentasRapidasDTO> spVentasRapidas(string id, string formaPagoId);
        Task<List<VentasRapidasIndex>> spVentasRapidasACobrar();
        Task<List<VentasRapidasIndex>> spVentasRapidasFacturadas();
        Task<VentasRapidasDTO> spVentasRapidas(string id);
        Task<VentasRapidasDTO> spVentasRapidasACobrar(string id);
        Task<VentasRapidasDTO> spVentasRapidasFacturadas(string id);
        Task<int> spInsertar(VentasRapidasDTO VentasRapidasDTO);
        Task<List<VentasRapidasDetalleDTO>> spDetalleVentaRapida(string ventaRapidaId);
        Task<VentasRapidasDetalleDTO> spDetalleVentaRapidaId(string Id);
        Task AddItemAsync(VentasRapidasDetalleDTO model, string userName);
        Task AddItemComodinAsync(VentasRapidasDetalleDTO model, string userName);
        Task DeleteDetailAsync(string id);
        Task ModifyCantidadesAsync(string id, int cantidad);
        Task<int> spDescuentoAplica(string ventaRapidaId, string descuentoId, string usuario);
        Task<int> spDescuentoBorrar(string Id, string usuario);
        Task<int> spClienteEditar(VentasRapidasDTO VentasRapidasDTO);
        Task<int> spEfectivo(FormaPagoEfectivoDTO formaPago);
        Task<int> spOtro(FormaPagoOtroDTO formaPago);
        Task<int> spTarjeta(FormaPagoTarjetaDTO formaPago);
        Task<int> spCheque(FormaPagoChequeDTO formaPago);
        Task<int> spDolar(FormaPagoDolarDTO formaPago);
        Task<List<VentasRapidasFormasPagosDTO>> spFormasPagos(string VentaRapidaId);
        Task<int> spGenerarVentaRapida(VentasRapidasDTO ventas);
        Task<int> spDelete(string ventaRapidaId);
        Task<int> spDeleteFormaPago(string id);
        Task<int> spGenerarCobrazaVentaRapida(VentasRapidasDTO ventas);

        Task<VentasRapidasResumen> spResumenVentaRapida(string ventaRapidaId);

        VentasRapidasDTO spVentasRapidasFacturadasPrint(string id);
        List<VentasRapidasDetalleDTO> spDetalleVentaRapidaPrint(string ventaRapidaId);
        List<VentasRapidasFormasPagosDTO> spFormasPagosPrint(string VentaRapidaId);
        VentasRapidasResumen spResumenVentaRapidaPrint(string ventaRapidaId);
    }
}
