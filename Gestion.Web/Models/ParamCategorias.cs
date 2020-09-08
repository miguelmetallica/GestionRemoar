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
        [MaxLength(150)]
        public string Descripcion { get; set; }        
        public bool Defecto { get; set; }
        public bool Estado { get; set; }

    }
}
