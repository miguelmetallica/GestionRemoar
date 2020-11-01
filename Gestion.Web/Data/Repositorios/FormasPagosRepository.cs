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

        
    }
}
