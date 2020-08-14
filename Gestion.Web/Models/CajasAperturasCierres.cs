using System;
using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public class CajasAperturasCierres : IEntidades
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un {0}")]
        [Display(Name = "Caja")]
        public string CajaId { get; set; }
        public Cajas Caja { get; set; }

        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha de Apertura")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaApertura { get; set; }
        public string UsuarioAperturaId { get; set; }
        public Usuarios UsuarioApertura { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public decimal EfectivoApertura { get; set; }
        public decimal DolaresApertura { get; set; }
        public decimal CuponesApertura { get; set; }
        public decimal ChequesApertura { get; set; }
        public decimal OtrosApertura { get; set; }
        public string ObservacionesApertura { get; set; }
        
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha de Cierre")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaCierre { get; set; }
        public string UsuarioCierreId { get; set; }
        public Usuarios UsuarioCierre { get; set; }
        public decimal? EfectivoCierre { get; set; }
        public decimal? DolaresCierre { get; set; }
        public decimal? CuponesCierre { get; set; }
        public decimal? ChequesCierre { get; set; }
        public decimal? OtrosCierre { get; set; }
        public string ObservacionesCierre { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaAlta { get; set; }

    }
}
