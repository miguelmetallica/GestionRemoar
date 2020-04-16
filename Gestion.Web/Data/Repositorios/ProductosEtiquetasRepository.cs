using Gestion.Web.Models;

namespace Gestion.Web.Data
{
    public class ProductosEtiquetasRepository : GenericRepository<ProductosEtiquetas>, IProductosEtiquetasRepository
    {
        public ProductosEtiquetasRepository(DataContext context) : base(context)
        {

        }
    }
}
