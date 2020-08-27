using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Web.Data
{
    public class TiposResponsablesRepository : GenericRepository<ParamTiposResponsables>, ITiposResponsablesRepository
    {
        private readonly DataContext context;
        public TiposResponsablesRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetCombo()
        {
            var list = this.context.ParamTiposResponsables.Where(x => x.Estado == true).Select(c => new SelectListItem
            {
                Text = c.Descripcion,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona una Tipo de Responsable...)",
                Value = ""
            });

            return list;
        }
    }
}
