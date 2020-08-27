using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Web.Data
{
    public class TiposComprobantesRepository : GenericRepository<ParamTiposComprobantes>, ITiposComprobantesRepository
    {
        private readonly DataContext context;
        public TiposComprobantesRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetCombo()
        {
            var list = this.context.ParamTiposComprobantes.Where(x => x.Estado == true).Select(c => new SelectListItem
            {
                Text = c.Descripcion,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona una Tipo de Comprobante...)",
                Value = ""
            });

            return list;
        }
    }
}
