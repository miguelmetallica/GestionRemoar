using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class ParamTiposProductos : IEntidades
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(6)]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150)]
        public string Descripcion { get; set; }
        public bool Defecto { get; set; }
        public bool Estado { get; set; }
    }
}
