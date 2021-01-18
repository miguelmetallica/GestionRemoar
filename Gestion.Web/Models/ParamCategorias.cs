using System;
using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class ParamCategorias : IEntidades
    {
        public string Id { get; set; }
        [Display(Name = "Categoria Padre")]
        public string PadreId { get; set; }
        public ParamCategorias Padre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(5)]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Descuento %")]
        [Range(0,100, ErrorMessage = "El campo {0} acepta valores entre {1} y {2}.")]
        public decimal DescuentoPorcentaje { get; set; }
        public bool Defecto { get; set; }
        public bool Estado { get; set; }

    }
}
