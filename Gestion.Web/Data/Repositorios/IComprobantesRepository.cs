using Gestion.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IComprobantesRepository : IGenericRepository<Comprobantes>
    { 
        Task<List<Comprobantes>> spComprobantes(string clienteId);
        Task<ComprobantesDTO> spComprobante(string Id);
        Task<List<Comprobantes>> spComprobanteImputacion(string Id);
        Task<int> spRecibo(ComprobantesReciboDTO reciboDTO);
        Task<int> spEfectivo(ComprobantesEfectivoDTO reciboDTO);
        Task<int> spOtro(ComprobantesOtroDTO reciboDTO);
        Task<int> spTarjeta(ComprobantesTarjetaDTO reciboDTO);
        Task<int> spCheque(ComprobantesChequeDTO reciboDTO);
        Task<int> spDolar(ComprobantesDolarDTO formaPago);
        Task<int> spDeleteFormaPago(string id);
        Task<List<ComprobantesFormasPagosDTO>> spComprobantesTmpFormasPagos(string clienteId);
        Task<List<ComprobantesFormasPagosDTO>> spComprobantesFormasPagos(string Id);
        Task<List<ComprobantesDTO>> spComprobantesPresupuestos(); 
        Task<List<ComprobantesFormasPagosDTO>> spComprobantesFormasPagosImprimir(string Id);
        Task<int> spComprobantesReciboAnular(string Id,string Motivo,string Usuario);
        Task<List<ComprobantesDetalleImputaDTO>> spComprobanteDetalleImputacion(string Id);
        Task<List<ComprobantesDetalleImputaDTO>> spComprobanteDetalleEntrega(string Id);
        Task<List<ComprobantesDetalleDTO>> spComprobanteDetalleImputa(string ProductoId);
        Task<int> spComprobanteDetalleInsertImputacion(ComprobantesDetalleDTO detalleDTO);
        Task<int> spComprobanteDetalleInsertEntrega(ComprobantesDetalleDTO detalleDTO);
        Task<int> spComprobanteDetalleInsertEntregaAnula(ComprobantesDetalleDTO detalleDTO);
        Task<int> spComprobanteDetalleInsertAutoriza(ComprobantesDetalleDTO detalleDTO);
        Task<int> spComprobanteDetalleInsertAutorizaAnula(ComprobantesDetalleDTO detalleDTO);
        Task<List<ComprobantesDTO>> spPresupuestosComprobantesEntrega();
        Task<int> spComprobanteDetalleInsertEntregaTMP(ComprobantesDetalleDTO detalleDTO);
        Task<List<ComprobantesDetalleDTO>> spComprobanteDetalleTMP(string ComprobanteId);
        Task<int> spComprobanteDetalleEliminarEntregaTMP(ComprobantesDetalleDTO detalleDTO);

        Task<int> spRemito(ComprobantesDetalleDTO detalleDTO);
        Task<List<ComprobantesDetalleDTO>> spComprobanteDetalleGet(string ComprobanteId);
        Task<List<ComprobantesDetalleDTO>> spComprobanteDetallePresupuestoGet(string presupuestoId);
        Task<List<ComprobantesDetalleDTO>> spComprobanteDetalleImprimirGet(string ComprobanteId);

        Task<int> spComprobanteInsertaDatosFiscales(ComprobantesDTO comprobantesDTO);
        Task<int> spComprobanteInsertaCodigoAutorizacion(ComprobantesFormasPagosDTO comprobantesDTO);
        Task<List<ComprobantesDetalleIndicador>> spComprobanteDetalleEntregaIndicador();
        Task<List<ComprobantesDTO>> spComprobantesPresupuesto(string PresupuestoId);
        Task<List<ComprobantesDetalleDTO>> spComprobantesDetalleGet(string ComprobanteId);

        Task<List<ComprobantesDTO>> spComprobantesPresupuestosCliente(string clienteId);
        Task<List<ComprobantesFormasPagosDTO>> spComprobantesFormasPagosPresupuesto(string presupuestoId);
        Task<List<ComprobantesDTO>> spPresupuestosComprobantesDevolucion();
        Task<int> spComprobanteDetalleInsertDevolucionTMP(ComprobantesDetalleDTO detalleDTO);
        Task<int> spDevolucion(ComprobantesDetalleDTO detalleDTO);
        Task<List<ComprobantesDetalleDTO>> spComprobanteDetalleDevolucion(string Id);
        Task<List<ComprobantesDTO>> spComprobantesImputaciones();
        Task<List<ComprobantesDetalleDTO>> spComprobanteRemitosDevolucionesGet(string presupuestoId);
        Task<List<ComprobantesDTO>> spComprobantesInputaciones(string ComprobanteId);
        Task<List<ComprobantesDTO>> spPresupuestosRemitos();
    }
}
