using System;
using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class FormasPagos : IEntidades
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(5)]
        public string Codigo { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150)]
        public string Descripcion { get; set; }
        public string Tipo { get; set; }

        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Vigencia Desde")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaDesde { get; set; }
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Vigencia Hasta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaHasta { get; set; }
        public bool Estado { get; set; }

    }

    public partial class FormasPagosCuotas : IEntidades
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]        
        [Display(Name = "Forma de Pago")]
        public string FormaPagoId { get; set; }

        [Display(Name = "Entidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]        
        public string EntidadId { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Cuota { get; set; }
        
        [Display(Name = "Interes %")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Interes { get; set; }

        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Vigencia Desde")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaDesde { get; set; }
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Vigencia Hasta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaHasta { get; set; }
        public bool Estado { get; set; }

    }
}
