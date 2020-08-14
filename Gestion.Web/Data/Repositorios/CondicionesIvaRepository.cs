using Gestion.Web.Models;

namespace Gestion.Web.Data
{
    public class CondicionesIvaRepository : GenericRepository<ParamCondicionesIva>, ICondicionesIvaRepository
    {
        public CondicionesIvaRepository(DataContext context) : base(context)
        {

        }
    }
}
