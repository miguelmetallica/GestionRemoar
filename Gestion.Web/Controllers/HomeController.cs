using Gestion.Web.Models;
using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPresupuestosRepository presupuesto;
        private readonly IComprobantesRepository comprobantes;

        public HomeController(ILogger<HomeController> logger,IPresupuestosRepository presupuesto,IComprobantesRepository comprobantes)
        {
            _logger = logger;
            this.presupuesto = presupuesto;
            this.comprobantes = comprobantes;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["presupuesto"] = await presupuesto.spPresupuestosPendientes();
            ViewData["comprobantePendiente"] = await comprobantes.spComprobanteDetalleEntregaIndicador();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

    }
}
