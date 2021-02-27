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
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string CajaId { get; set; }
        public ParamCajas Caja { get; set; }

        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Fecha { get; set; }
        
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string TipoMovimientoId { get; set; }
        public ParamCajasMovimientosTipos TipoMovimiento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Importe { get; set; }
        public string Observaciones { get; set; }

        public DateTime FechaAlta { get; set; }
        public string UsuarioAlta { get; set; }

        public string SucursalId { get; set; }
        public Sucursales Sucursal { get; set; }

    }

    public class CajasMovimientosAdministra : IEntidades
    {
        public string Id { get; set; }

        [Display(Name = "Caja")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string CajaId { get; set; }
        public ParamCajas Caja { get; set; }

        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime? Fecha { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string TipoMovimientoId { get; set; }
        public ParamCajasMovimientosTipos TipoMovimiento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Importe { get; set; }
        public string Observaciones { get; set; }

        public DateTime FechaAlta { get; set; }
        public string UsuarioAlta { get; set; }
        public string SucursalId { get; set; }
        public Sucursales Sucursal { get; set; }
    }
}
