using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public interface IFormasPagosCuotasRepository : IGenericRepository<FormasPagosCuotas>
    {
        IEnumerable<SelectListItem> GetCombo();
        List<FormasPagosCuotas> GetAll(string formaPagoId);
        IEnumerable<SelectListItem> GetBancos();
        IEnumerable<SelectListItem> GetEntidades(string formaPagoId);
        IEnumerable<SelectListItem> GetCuotas(string formaPagoId, string entidadId);
        IEnumerable<SelectListItem> GetCuotasInteres(string formaPagoId, string entidadId, int cuota);
        FormasPagosCuotas GetCuotaUno(string formaPagoId);
    }    
}
