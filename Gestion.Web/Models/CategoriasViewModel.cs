using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public class CategoriasViewModel:Categorias
    {
        [Display(Name = "Image")]
        public IFormFile ImagenFile { get; set; }
    }
}
