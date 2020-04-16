using Gestion.Web.Models;

namespace Gestion.Web.Data
{
    public class EtiquetasRepository : GenericRepository<Etiquetas>, IEtiquetasRepository
    {
        public EtiquetasRepository(DataContext context) : base(context)
        {

        }
    }
}
