using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface ICajasRepository : IGenericRepository<ParamCajas>
    {
        IEnumerable<SelectListItem> GetCombo(string sucursalId);
        Task<List<CajasEstadoDTO>> spCajasEstadoImportesGet(string fecha, string sucursalId);
        Task<List<CajasEstadoDTO>> spCajasEstadoUsuariosGet(string fecha, string sucursalId);
        Task<List<CajasEstadoDTO>> spCajasEstadoFechaGet();
    }
}
