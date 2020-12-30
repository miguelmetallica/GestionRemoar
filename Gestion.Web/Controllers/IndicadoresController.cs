using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,Indicadores")]
    
    public class IndicadoresController : Controller
    {
        private readonly IUserHelper userHelper;
        private readonly IComprobantesRepository comprobantesRepository;

        public IndicadoresController(IUserHelper userHelper,IComprobantesRepository comprobantesRepository)
        {
            this.userHelper = userHelper;
            this.comprobantesRepository = comprobantesRepository;
        }

        public async Task<IActionResult> EntregasPendientes()
        {
            var model = await comprobantesRepository.spComprobanteDetalleEntregaIndicador();
            return View(model);
        }
        
    }
}
