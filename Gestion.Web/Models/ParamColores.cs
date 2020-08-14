using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class ParamColores : IEntidades
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(5)]
        public string Codigo { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150)]
        public string Descripcion { get; set; }
        
        [MaxLength(50)]
        public string Color { get; set; }
        public bool Estado { get; set; }

    }
}
