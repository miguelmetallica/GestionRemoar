using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IProductosRepository : IGenericRepository<Productos>
    {
        IEnumerable<SelectListItem> GetComboProducts();
        Task<bool> ExistCodigoAsync(string id, string codigo);
        Task<int> spInsertar(ProductosDTO productos);
        Task<int> spEditar(ProductosDTO productos);
        Task<List<ProductosIndex>> spProductosGet();
        Task<List<ProductosCatalogo>> spCatalogoProductosGet();

        Task<ProductosDTO> spProductosOne(string Id);
        Task<ProductosDTO> spProductosOneCodigo(string Codigo);
        Task<List<ProductosIndex>> spProductosVentaGet();

        Task<ProductosIndex> spProductosVentaGetOne(string Id);
    }
}
