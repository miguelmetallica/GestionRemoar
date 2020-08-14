using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Gestion.Web.Data
{
    public interface ICajasRepository : IGenericRepository<Cajas>
    {
        IEnumerable<SelectListItem> GetCombo(string sucursalId);
    }
}
