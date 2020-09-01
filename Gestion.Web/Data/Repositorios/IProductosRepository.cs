using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IProductosRepository : IGenericRepository<Productos>
    {
        IEnumerable<SelectListItem> GetComboProducts();

        Task<Productos> GetProducto(string id);

        Task<bool> ExistCodigoAsync(string id, string codigo);

        Task<int> spInsertar(Productos productos);

        Task<int> spEditar(Productos productos);
        Task<List<ProductosIndex>> spProductosGet();
    }
}
