using Gestion.Web.Models;
using System.Linq;

namespace Gestion.Web.Data
{
    public interface ICajasAperturasCierresRepository : IGenericRepository<CajasAperturasCierres>
    {
        IQueryable<CajasAperturasCierres> GetCajasAll();
    }
}
