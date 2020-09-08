using Gestion.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public class ProductosImagenesRepository : GenericRepository<ProductosImagenes>, IProductosImagenesRepository
    {
        private readonly DataContext context;

        public ProductosImagenesRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ProductosImagenes>> GetImagenes(string id)
        {
            var list =await this.context.ProductosImagenes.Where(x => x.ProductoId == id).ToListAsync();
            return list;
        }

        public async Task DeleteImage(ProductosImagenes productosImagenes)
        {
            this.context.Set<ProductosImagenes>().Remove(productosImagenes);
            await this.context.SaveChangesAsync();            
        }
    }
}
