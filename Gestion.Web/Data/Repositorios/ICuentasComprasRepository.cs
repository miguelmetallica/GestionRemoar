using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Gestion.Web.Data
{
    public interface ICuentasComprasRepository : IGenericRepository<ParamCuentasCompras>
    {
        IEnumerable<SelectListItem> GetCombo();
    }
}
