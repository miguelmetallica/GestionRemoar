using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Web.Data
{
    public class PresupuestosEstadosRepository : GenericRepository<ParamPresupuestosEstados>, IPresupuestosEstadosRepository
    {
        private readonly DataContext context;

        public PresupuestosEstadosRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetCombo()
        {
            var list = this.context.ParamPresupuestosEstados.Select(c => new SelectListItem
            {
                Text = c.Descripcion,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un Estado...)",
                Value = ""
            });

            return list;
        }
    }
}
