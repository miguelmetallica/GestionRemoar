using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Web.Data
{
    public interface ICategoriasRepository : IGenericRepository<ParamCategorias>
    {
        IEnumerable<SelectListItem> GetCombo();

        IQueryable<ParamCategorias> GetCategorias();
    }
}
