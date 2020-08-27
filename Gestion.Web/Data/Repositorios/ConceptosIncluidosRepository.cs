using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Web.Data
{
    public class ConceptosIncluidosRepository : GenericRepository<ParamConceptosIncluidos>, IConceptosIncluidosRepository
    {
        private readonly DataContext context;
        public ConceptosIncluidosRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetCombo()
        {
            var list = this.context.ParamConceptosIncluidos.Where(x => x.Estado == true).Select(c => new SelectListItem
            {
                Text = c.Descripcion,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona una Tipo de Concepto...)",
                Value = ""
            });

            return list;
        }
    }
}
