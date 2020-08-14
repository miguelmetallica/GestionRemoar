using Gestion.Web.Models;

namespace Gestion.Web.Data
{
    public class ComprobantesTiposRepository : GenericRepository<ParamComprobantesTipos>, IComprobantesTiposRepository
    {
        public ComprobantesTiposRepository(DataContext context) : base(context)
        {

        }
    }
}
