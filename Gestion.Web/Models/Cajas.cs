using System;
using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    //public partial class Cajas : IEntidades
    //{
    //    public string Id { get; set; }

    //    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    //    [MaxLength(5)]
    //    public string Codigo { get; set; }

    //    [Required(ErrorMessage = "Debes seleccionar un {0}")]
    //    [Display(Name = "Sucursal")]
    //    public string SucursalId { get; set; }
    //    public string Sucursal { get; set; }

    //    public bool Estado { get; set; }
    //    public DateTime FechaAlta { get; set; }
    //    public string UsuarioAlta { get; set; }

    //}

    public partial class CajasEstadoDTO 
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        [Display(Name = "Tipos de Pagos")]
        public string FormaPagoTipo { get; set; }
        public decimal Total { get; set; }
        public string Sucursal { get; set; }
        public string SucursalId { get; set; }
        public string Usuario { get; set; }
        public string Id { get; set; }
    }
}
