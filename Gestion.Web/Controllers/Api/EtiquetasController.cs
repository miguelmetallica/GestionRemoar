using Gestion.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace Gestion.Web.Controllers.Api
{
    [Route("api/[Controller]")]
    public class EtiquetasController : Controller
    {
        private readonly IEtiquetasRepository etiquetasRepository;

        public EtiquetasController(IEtiquetasRepository etiquetasRepository)
        {
            this.etiquetasRepository = etiquetasRepository;
        }
        [HttpGet]
        public IActionResult GetEtiquetas() 
        {
            return Ok(this.etiquetasRepository.GetAll());
        }
    }
}
