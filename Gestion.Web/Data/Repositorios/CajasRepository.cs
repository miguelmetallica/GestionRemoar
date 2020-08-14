using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public class CajasRepository : GenericRepository<Cajas>, ICajasRepository
    {
        private readonly DataContext context;

        public CajasRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetCombo(string sucursalId)
        {
            var list = this.context.Cajas
                .Where(x => x.SucursalId == sucursalId)
                .Select(c => new SelectListItem
            {
                Text = c.Codigo,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona una Caja...)",
                Value = ""
            });

            return list;
        }
    }
}
