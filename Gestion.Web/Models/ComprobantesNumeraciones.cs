using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class ComprobantesNumeraciones : IEntidades
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Sucursal")]
        public string SucursalId { get; set; }
        public Sucursales Sucursal { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Tipo Comprobante")]
        public string TipoComprobanteId { get; set; }
        public ParamTiposComprobantes TipoComprobante { get; set; }
        public string Letra { get; set; }
        [Display(Name = "Punto de Venta")]
        public int PuntoVenta { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal Numero { get; set; }
        public bool Estado { get; set; }
    }
}
