using Gestion.Web.Models;

namespace Gestion.Web.Data
{
    public class CategoriasRepository : GenericRepository<Categorias>, ICategoriasRepository
    {
        public CategoriasRepository(DataContext context) : base(context)
        {

        }
    }
}
