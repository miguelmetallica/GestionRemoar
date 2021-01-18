using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Web.Data
{
    public class CategoriasRepository : GenericRepository<ParamCategorias>, ICategoriasRepository
    {
        private readonly DataContext context;

        public CategoriasRepository(DataContext context) : base(context)
        {

            this.context = context;            
        }

        public IEnumerable<SelectListItem> GetCombo()
        {
            var list = this.context.ParamCategorias
                .Where(x => x.Estado == true)
                .Select(c => new SelectListItem
                {
                    Text = c.Codigo.ToString() + " - " + c.Descripcion.ToString(),
                    Value = c.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona una Categoria)",
                Value = ""
            });

            return list;
        }

        public IQueryable<ParamCategorias> GetCategorias()
        {
            return this.context.ParamCategorias
                .Include(c => c.Padre)                
                ;
        }
    }
}
