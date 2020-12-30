using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Web.Data
{
    public class CajasTiposMovimientosRepository : GenericRepository<ParamCajasMovimientosTipos>, ICajasTiposMovimientosRepository
    {
        private readonly DataContext context;

        public CajasTiposMovimientosRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetCombo()
        {
            var list = this.context.ParamCajasMovimientosTipos.Where(c => c.Estado == true).Select(c => new SelectListItem
            {
                Text = c.Descripcion,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un Tipo de Mov...)",
                Value = ""
            });

            return list;
        }
    }
}
