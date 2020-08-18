using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class SistemaConfiguraciones : IEntidades
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150)]
        public string Configuracion { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150)]
        [Display(Name = "Valor")]
        public string Valor { get; set; }
        
        public bool Estado { get; set; }

    }
}
