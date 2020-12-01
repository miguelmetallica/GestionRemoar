using Gestion.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IComprobantesRepository : IGenericRepository<Comprobantes>
    {
        Task<List<Comprobantes>> spComprobantes(string clienteId);

        Task<int> spRecibo(ComprobantesReciboDTO reciboDTO);
        Task<int> spEfectivo(ComprobantesEfectivoDTO reciboDTO);
        Task<int> spOtro(ComprobantesOtroDTO reciboDTO);
        Task<int> spTarjeta(ComprobantesTarjetaDTO reciboDTO);
        Task<int> spCheque(ComprobantesChequeDTO reciboDTO);        
        Task<int> spDeleteFormaPago(string id);
        Task<List<ComprobantesFormasPagosDTO>> spComprobantesTmpFormasPagos(string clienteId);
    }
}
