using Gestion.Web.Models;

namespace Gestion.Web.Data
{
    public class ColoresRepository : GenericRepository<ParamColores>, IColoresRepository
    {
        public ColoresRepository(DataContext context) : base(context)
        {

        }
    }
}
