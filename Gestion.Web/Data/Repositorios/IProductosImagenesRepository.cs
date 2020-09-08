using Gestion.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IProductosImagenesRepository : IGenericRepository<ProductosImagenes>
    {
        Task<IEnumerable<ProductosImagenes>> GetImagenes(string id);

        Task DeleteImage(ProductosImagenes productosImagenes);        
    }
}
