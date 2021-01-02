using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Web.Data
{
    public class ProveedoresRepository : GenericRepository<Proveedores>, IProveedoresRepository
    {
        private readonly DataContext context;

        public ProveedoresRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetCombo()
        {
            var list = this.context.Proveedores.Where( x => x.Estado == true ).Select(c => new SelectListItem
            {
                Text = c.RazonSocial,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un Proveedor...)",
                Value = ""
            });

            return list;
        }
    }
}
