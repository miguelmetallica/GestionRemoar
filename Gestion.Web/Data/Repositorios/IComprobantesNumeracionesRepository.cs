using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IComprobantesNumeracionesRepository : IGenericRepository<ComprobantesNumeraciones>
    {
        //IEnumerable<SelectListItem> GetCombo();

        Task<List<ComprobantesNumeraciones>> GetTodos();
    }
}
