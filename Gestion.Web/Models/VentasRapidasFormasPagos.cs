using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Gestion.Web.Models
{
    public partial class VentasRapidasFormasPagos : IEntidades
    {
        public string Id { get; set; }
        public string ComprobanteId { get; set; }
        public string ClienteId { get; set; }
        public string FormaPagoId { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        public decimal Importe { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Forma de Pago")]
        public string FormaPago { get; set; }

        [Display(Name = "Codigo de Autorizacion")]
        public string CodigoAutorizacion { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }

    }

    public partial class VentasRapidasFormasPagosDTO
    {
        public string Id { get; set; }
        public string ClienteId { get; set; }
        public string VentaRapidaId { get; set; }
        public string TipoComprobanteId { get; set; }
        public string FormaPagoId { get; set; }
        public string FormaPagoCodigo { get; set; }
        public string FormaPagoTipo { get; set; }
        public string FormaPago { get; set; }
        public decimal Importe { get; set; }
        public int Cuota { get; set; }
        public decimal Interes { get; set; }
        public decimal Descuento { get; set; }        
        public decimal Total { get; set; }
        public string TarjetaId { get; set; }
        public string TarjetaNombre { get; set; }
        public string TarjetaCliente { get; set; }
        public string TarjetaNumero { get; set; }
        public int TarjetaVenceMes { get; set; }
        public int TarjetaVenceAño { get; set; }
        public int TarjetaCodigoSeguridad { get; set; }
        public bool? TarjetaEsDebito { get; set; }
        public string ChequeBancoId { get; set; }
        public string ChequeBanco { get; set; }
        public string ChequeNumero { get; set; }
        public DateTime? ChequeFechaEmision { get; set; }
        public DateTime? ChequeFechaVencimiento { get; set; }
        public string ChequeCuit { get; set; }
        public string ChequeNombre { get; set; }
        public string ChequeCuenta { get; set; }
        public string Otros { get; set; }
        public string Observaciones { get; set; }
        [Display(Name = "Codigo de Autorizacion")]
        public string CodigoAutorizacion { get; set; }
        public decimal DolarImporte { get; set; }
        public decimal DolarCotizacion { get; set; }
        public DateTime FechaAlta { get; set; }
        public string UsuarioAlta { get; set; }

    }

    public partial class FormaPagoEfectivoDTO
    {
        public string VentaRapidaId { get; set; }
        public string ClienteId { get; set; }
        public string TipoComprobanteId { get; set; }
        public string FormaPagoId { get; set; }
       
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        public decimal Importe { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Descuento { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El Saldo debe ser mayor a Cero")]
        public decimal Saldo { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SaldoConDescuento { get; set; }

    }

    public partial class FormaPagoDolarDTO
    {
        public string VentaRapidaId { get; set; }
        public string ClienteId { get; set; }
        public string TipoComprobanteId { get; set; }
        public string FormaPagoId { get; set; }
        [Display(Name = "Importe en Pesos")]
        public decimal Importe { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        [Display(Name = "Importe en Dolares")]
        public decimal DolarImporte { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        [Display(Name = "Cotizacion")]
        public decimal DolarCotizacion { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Descuento { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El Saldo debe ser mayor a Cero")]
        public decimal Saldo { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SaldoConDescuento { get; set; }

    }

    public partial class FormaPagoTarjetaDTO
    {
        public string VentaRapidaId { get; set; }
        public string ClienteId { get; set; }
        public string FormaPagoId { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        public decimal Importe { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        //[Range(1, 9999999, ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        [Display(Name ="Nro de Cuota")]
        public int Cuota { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.00, 9999999, ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        [Display(Name = "Tasa de Interes %")]
        public decimal Interes { get; set; }

        [Display(Name = "Total Cuota")]
        [DisplayFormat(DataFormatString = "{0:C2}")]               
        public decimal Total { get; set; }

        [Display(Name = "Tarjeta")]
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string TarjetaId { get; set; }
        [Display(Name = "Tarjeta")]
        public string TarjetaNombre { get; set; }
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Nombre del Cliente")]
        public string TarjetaCliente { get; set; }
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Numero de Tarjeta")]
        public string TarjetaNumero { get; set; }
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Mes Vence")]
        public string TarjetaVenceMes { get; set; }
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Año Vence")]
        public string TarjetaVenceAño { get; set; }
        [Display(Name = "Codigo Seguridad")]
        public string TarjetaCodigoSeguridad { get; set; }
        public bool TarjetaEsDebito { get; set; }
        [Display(Name = "Codigo de Autorizacion")]
        public string CodigoAutorizacion { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Descuento { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Recargo { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El Saldo debe ser mayor a Cero")]
        public decimal Saldo { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SaldoConDescuento { get; set; }
    }

    public partial class FormaPagoChequeDTO
    {
        public string VentaRapidaId { get; set; }
        public string ClienteId { get; set; }
        public string FormaPagoId { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        public decimal Importe { get; set; }

        [Display(Name = "Banco")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ChequeBancoId { get; set; }
        [Display(Name = "Banco")]
        public string ChequeBanco { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ChequeNumero { get; set; }
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha de Emision")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime?ChequeFechaEmision { get; set; }
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha de Vencimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime? ChequeFechaVencimiento { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ChequeCuit { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ChequeNombre { get; set; }
        public string ChequeCuenta { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Descuento { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Recargo { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El Saldo debe ser mayor a Cero")]
        public decimal Saldo { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SaldoConDescuento { get; set; }
    }

    public partial class FormaPagoOtroDTO
    {
        public string VentaRapidaId { get; set; }
        public string ClienteId { get; set; }
        public string FormaPagoId { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        public decimal Importe { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Forma de Pago")]
        public string FormaPago{ get; set; }

        [Display(Name = "Codigo de Autorizacion")]
        public string CodigoAutorizacion { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }

    }

    
}
