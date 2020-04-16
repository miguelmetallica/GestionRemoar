using Gestion.Web.Models;

namespace Gestion.Web.Data
{
    public class TiposDocumentosRepository : GenericRepository<ParamTiposDocumentos>, ITiposDocumentosRepository
    {
        public TiposDocumentosRepository(DataContext context) : base(context)
        {

        }
    }
}
