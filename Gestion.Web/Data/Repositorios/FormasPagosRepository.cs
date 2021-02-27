using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Web.Data
{
    public class FormasPagosRepository : GenericRepository<FormasPagos>, IFormasPagosRepository
    {
        private readonly DataContext context;
        public FormasPagosRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetTipos()
        {
            List<SelectListItem> lst = new List<SelectListItem>();

            lst.Add(new SelectListItem() { Text = "(Selecciona un Tipo...)", Value = "" });
            lst.Add(new SelectListItem() { Text = "Efectivo", Value = "E" });
            lst.Add(new SelectListItem() { Text = "Tarjeta de Debito", Value = "D" });
            lst.Add(new SelectListItem() { Text = "Tarjeta de Credito", Value = "C" });
            lst.Add(new SelectListItem() { Text = "Cheques", Value = "X" });
            lst.Add(new SelectListItem() { Text = "Dolares", Value = "U" });
            lst.Add(new SelectListItem() { Text = "Otros", Value = "O" });
            
            return lst;
        }

    }
}
