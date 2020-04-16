using Gestion.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace Gestion.Web.Controllers.Api
{
    [Route("api/[Controller]")]
    public class ProductosController : Controller
    {
        private readonly ITiposDocumentosRepository tiposDocumentosRepository;

        public ProductosController(ITiposDocumentosRepository tiposDocumentosRepository)
        {
            this.tiposDocumentosRepository = tiposDocumentosRepository;
        }
        [HttpGet]
        public IActionResult GetTiposDocumentos() 
        {
            return Ok(this.tiposDocumentosRepository.GetAll());
        }
    }
}
