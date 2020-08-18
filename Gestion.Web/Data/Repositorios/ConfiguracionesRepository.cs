using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Web.Data
{
    public class ConfiguracionesRepository : GenericRepository<SistemaConfiguraciones>, IConfiguracionesRepository
    {
        public ConfiguracionesRepository(DataContext context) : base(context)
        {

        }

    }
}
