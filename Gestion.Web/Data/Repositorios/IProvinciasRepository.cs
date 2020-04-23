using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IProvinciasRepository : IGenericRepository<Provincia>
    {
        IQueryable GetProvinciasWithLocalidades();

        Task<Provincia> GetProvinciasWithLocalidadesAsync(int id);

        Task<Localidades> GetLocalidadesAsync(int id);

        Task AddLocalidadesAsync(LocalidadesViewModel model);

        Task<int> UpdateLocalidadAsync(Localidades localidades);

        Task<int> DeleteLocalidadAsync(Localidades localidades);

        IEnumerable<SelectListItem> GetComboProvincias();

        IEnumerable<SelectListItem> GetComboLocalidades(int localidadId);

        Task<Provincia> GetProvinciasAsync(Localidades localidades);


    }
}
