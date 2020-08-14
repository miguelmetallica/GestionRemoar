using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class Provincia : IEntidades
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Codigo")]
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Codigo { get; set; }
        
        [Required]
        [Display(Name = "Provincia")]
        [MaxLength(250, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Nombre { get; set; }

        public ICollection<Localidades> Localidades { get; set; }

        [Display(Name = "# Localidades")]
        public int NumeroLocalidades { get { return this.Localidades == null ? 0 : this.Localidades.Count; } }

    }
}
