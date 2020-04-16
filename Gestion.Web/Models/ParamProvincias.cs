using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class ParamProvincias : IEntidades
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Descripcion { get; set; }        
        public bool Estado { get; set; }
        public Usuarios User { get; set; }
    }
}
