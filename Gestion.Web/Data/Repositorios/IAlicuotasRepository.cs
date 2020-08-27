using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IAlicuotasRepository : IGenericRepository<ParamAlicuotas>
    {
        IEnumerable<SelectListItem> GetCombo();
    }    
}
