using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Web.Data
{
    public class PresupuestosDescuentosRepository : GenericRepository<ParamPresupuestosDescuentos>, IPresupuestosDescuentosRepository
    {
        private readonly DataContext context;

        public PresupuestosDescuentosRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetCombo()
        {
            var list = this.context.ParamPresupuestosDescuentos.Where( x => x.Estado == true ).Select(c => new SelectListItem
            {
                Text = c.Descripcion,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona el Descuento...)",
                Value = ""
            });

            return list;
        }
    }
}
