using Gestion.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface ICajasMovimientosRepository : IGenericRepository<CajasMovimientos>
    {
        Task<IQueryable<CajasMovimientos>> GetMovimientos(string usuario);
        Task<IQueryable<CajasMovimientos>> GetMovimientosAll(string usuario);
        Task<int> spPagoProveedorInsert(CajasPagoProveedorDTO item);
    }
}
