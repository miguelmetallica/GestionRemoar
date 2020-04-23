using Gestion.Web.Models;

namespace Gestion.Web.Data
{
    public class LocalidadesRepository : GenericRepository<Localidades>, ILocalidadesRepository
    {
        public LocalidadesRepository(DataContext context) : base(context)
        {

        }
    }
}
