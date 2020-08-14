using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class Localidades : IEntidades
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Codigo")]
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Codigo { get; set; }

        [Required]
        [Display(Name = "Localidad")]
        [MaxLength(250, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Codigo Postal")]
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string CodigoPostal { get; set; }
                
    }
}
