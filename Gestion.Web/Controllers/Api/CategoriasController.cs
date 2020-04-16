using Gestion.Web.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestion.Web.Controllers.Api
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriasController : Controller
    {

        private readonly ICategoriasRepository categoriasRepository;

        public CategoriasController(ICategoriasRepository categoriasRepository)
        {
            this.categoriasRepository = categoriasRepository;
        }
        [HttpGet]
        public IActionResult GetEtiquetas() 
        {
            return Ok(this.categoriasRepository.GetAll());
        }
    }
}
