using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class ParamPresupuestosDescuentos : IEntidades
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150)]
        [Display(Name = "Descuento")]
        public string Descripcion { get; set; }
        public decimal Porcentaje { get; set; }
        public bool Estado { get; set; }

    }
}
