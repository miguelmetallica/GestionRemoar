using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion.Web.Models
{
    public class CajasMovimientos : IEntidades
    {
        public string Id { get; set; }

        [Display(Name = "Caja")]
        public string CajaId { get; set; }
        public Cajas Caja { get; set; }

        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Fecha { get; set; }
        
        [Display(Name = "Tipo")]
        public string TipoMovimientoId { get; set; }
        public ParamCajasMovimientosTipos TipoMovimiento { get; set; }

        public decimal Importe { get; set; }
        public string Observaciones { get; set; }

    }
}
