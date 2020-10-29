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

    }
}
