using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public class ComprobantesNumeracionesRepository : GenericRepository<ComprobantesNumeraciones>, IComprobantesNumeracionesRepository
    {
        private readonly DataContext context;

        public ComprobantesNumeracionesRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<ComprobantesNumeraciones>> GetTodos()
        {
            var comprobantesNumeraciones = await this.context.ComprobantesNumeraciones
                                    .Include(x => x.Sucursal)
                                    .Include(x => x.TipoComprobante)
                                    .ToListAsync();
                                    
            return comprobantesNumeraciones;
        }

        //public IEnumerable<SelectListItem> GetCombo()
        //{
        //    var list = this.context.ComprobantesNumeraciones.Where(x => x.Estado == true).Select(c => new SelectListItem
        //    {
        //        Text = c.Descripcion,
        //        Value = c.Id.ToString()
        //    }).OrderBy(l => l.Text).ToList();

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "(Selecciona un Tipo de Producto...)",
        //        Value = ""
        //    });

        //    return list;
        //}
    }
}
