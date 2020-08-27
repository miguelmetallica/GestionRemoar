using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Web.Data
{
    public class AlicuotasRepository : GenericRepository<ParamAlicuotas>, IAlicuotasRepository
    {
        private readonly DataContext context;
        public AlicuotasRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetCombo()
        {
            var list = this.context.ParamAlicuotas.Where(x => x.Estado == true).Select(c => new SelectListItem
            {
                Text = c.Descripcion,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona una Alicuota...)",
                Value = ""
            });

            return list;
        }
    }
}
