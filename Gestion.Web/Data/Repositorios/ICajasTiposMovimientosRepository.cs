using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface ICajasTiposMovimientosRepository : IGenericRepository<ParamCajasMovimientosTipos>
    {
        IEnumerable<SelectListItem> GetCombo();
        
    }
}
