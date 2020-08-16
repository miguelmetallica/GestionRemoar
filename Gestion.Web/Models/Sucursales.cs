using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class Sucursales : IEntidades
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Codigo")]
        [MaxLength(5, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Codigo { get; set; }
        
        [Required]
        [Display(Name = "Sucursal")]
        [MaxLength(150, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Nombre { get; set; }

        [Display(Name = "Provincia")]
        public string ProvinciaId { get; set; }
        public ParamProvincias Provincia { get; set; }
        //public ICollection<ParamProvincias> Provincia { get; set; }

        [Required]
        [Display(Name = "Localidad")]
        [MaxLength(250, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Localidad { get; set; }

        [Required]
        [Display(Name = "Codigo Postal")]
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string CodigoPostal { get; set; }

        [Required]
        [Display(Name = "Calle")]
        [MaxLength(500, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Calle { get; set; }

        [Required]
        [Display(Name = "Numero")]
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string CalleNro { get; set; }

        [Required]
        [Display(Name = "Piso / Dpto")]
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string PisoDpto { get; set; }

        [Required]
        [Display(Name = "Otras Referencias")]
        [MaxLength(500, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string OtrasReferencias { get; set; }
        public bool Estado { get; set; }

    }
}
