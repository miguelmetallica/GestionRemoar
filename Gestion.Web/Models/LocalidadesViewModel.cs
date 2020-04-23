using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Models
{
    public class LocalidadesViewModel
    {
        public int ProvinciaId { get; set; }

        public int LocalidadId { get; set; }

        [Required]
        [Display(Name = "Localidad")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Nombre { get; set; }


    }
}
