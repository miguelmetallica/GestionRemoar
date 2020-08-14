using Gestion.Web.Models;

namespace Gestion.Web.Data
{
    public class CajasMovimientosRepository : GenericRepository<CajasMovimientos>, ICajasMovimientosRepository
    {
        public CajasMovimientosRepository(DataContext context) : base(context)
        {

        }
    }
}
