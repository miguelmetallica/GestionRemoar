using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Data
{
    public class CajasMovimientosRepository : GenericRepository<CajasMovimientos>, ICajasMovimientosRepository
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;

        public CajasMovimientosRepository(DataContext context, 
            IUserHelper userHelper) : base(context)
        {
            this.context = context;
            this.userHelper = userHelper;
        }

        public async Task<IQueryable<CajasMovimientos>> GetMovimientos(string usuario)
        {
            var user = await userHelper.GetUserByEmailAsync(usuario);
            return this.context.CajasMovimientos.Where(x=> x.SucursalId == user.SucursalId)
                            .Include(x => x.Caja)
                            .Include(x => x.Caja.Sucursal)
                            .Include(x => x.TipoMovimiento);
        }

        public async Task<IQueryable<CajasMovimientos>> GetMovimientosAll(string usuario)
        {
            var user = await userHelper.GetUserByEmailAsync(usuario);
            return this.context.CajasMovimientos
                            .Include(x => x.Caja)
                            .Include(x => x.Caja.Sucursal)
                            .Include(x => x.TipoMovimiento);
        }
    }
}
