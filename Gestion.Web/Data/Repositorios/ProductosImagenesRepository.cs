using Gestion.Web.Models;

namespace Gestion.Web.Data
{
    public class ProductosImagenesRepository : GenericRepository<ProductosImagenes>, IProductosImagenesRepository
    {
        public ProductosImagenesRepository(DataContext context) : base(context)
        {

        }
    }
}
