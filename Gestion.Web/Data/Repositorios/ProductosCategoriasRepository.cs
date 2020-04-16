using Gestion.Web.Models;

namespace Gestion.Web.Data
{
    public class ProductosCategoriasRepository : GenericRepository<ProductosCategorias>, IProductosCategoriasRepository
    {
        public ProductosCategoriasRepository(DataContext context) : base(context)
        {

        }
    }
}
