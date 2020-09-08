using Gestion.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IProductosCategoriasRepository : IGenericRepository<ProductosCategorias>
    {
        Task<List<ProductosCategoriasDTO>> spProductosCategoriasGet(string id);

        Task<int> spProductosCategoriasOn(string id, string productoId, string categoriaId);

        Task<int> spProductosCategoriasOff(string productoId, string categoriaId);
    }
}
