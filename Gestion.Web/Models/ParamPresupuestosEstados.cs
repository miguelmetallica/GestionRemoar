using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class ParamPresupuestosEstados : IEntidades
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(5)]
        public string Codigo { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150)]
        [Display(Name = "Estado")]
        public string Descripcion { get; set; }
        
        public bool Estado { get; set; }

    }
}
