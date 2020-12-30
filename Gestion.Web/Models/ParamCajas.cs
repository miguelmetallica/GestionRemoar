using System;
using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class ParamCajas : IEntidades
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(5)]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un {0}")]
        [Display(Name = "Sucursal")]
        public string SucursalId { get; set; }
        public Sucursales Sucursal { get; set; }

        public bool Estado { get; set; }
        public DateTime FechaAlta { get; set; }
        public string UsuarioAlta { get; set; }

    }
    
}
