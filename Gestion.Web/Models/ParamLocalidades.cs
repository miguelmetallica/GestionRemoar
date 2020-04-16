using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion.Web.Models
{
    public partial class ParamLocalidades : IEntidades
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(6)]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(6)]
        public string CodigoPostal { get; set; }
        
        public bool Estado { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public ParamProvincias Provincia { get; set; }
        public Usuarios User { get; set; }
    }
}
