using Gestion.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Gestion.Web.Data
{
    public class CajasAperturasCierresRepository : GenericRepository<CajasAperturasCierres>, ICajasAperturasCierresRepository
    {
        private readonly DataContext context;

        public CajasAperturasCierresRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IQueryable<CajasAperturasCierres> GetCajasAll()
        {
            return this.context.CajasAperturasCierres
                .Include(c => c.Caja)
                .Include(c => c.Caja.Sucursal)
                .Include(c => c.UsuarioApertura)
                .Include(c => c.UsuarioCierre)
                ;
        }
        
    }
}
