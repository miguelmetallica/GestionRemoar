using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Web.Data
{
    public class CuentasComprasRepository : GenericRepository<ParamCuentasCompras>, ICuentasComprasRepository
    {
        private readonly DataContext context;

        public CuentasComprasRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetCombo()
        {
            var list = this.context.ParamCuentasCompras.Where(x => x.Estado == true).Select(c => new SelectListItem
            {
                Text = c.Descripcion,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona una Cuenta...)",
                Value = ""
            });

            return list;
        }
    }
}
