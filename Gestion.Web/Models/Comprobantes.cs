using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Gestion.Web.Models
{
    public partial class Comprobantes : IEntidades
    {
        [Key]
        public string Id { get; set; }
        public string Codigo { get; set; }
        public string TipoComprobanteId { get; set; }
        public string TipoComprobante { get; set; }
        public string TipoComprobanteCodigo { get; set; }
        public string PresupuestoId { get; set; }
        public string Letra { get; set; }
        public int PtoVenta { get; set; }
        public decimal Numero { get; set; }
        
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaComprobante { get; set; }
        public string ConceptoIncluidoId { get; set; }
        public string ConceptoIncluidoCodigo { get; set; }
        public string ConceptoIncluido { get; set; }
        public DateTime? PeriodoFacturadoDesde { get; set; }
        public DateTime? PeriodoFacturadoHasta { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string TipoResponsableId { get; set; }
        public string TipoResponsableCodigo { get; set; }
        public string TipoResponsable { get; set; }
        public string ClienteId { get; set; }
        public string ClienteCodigo { get; set; }
        public string TipoDocumentoId { get; set; }
        public string TipoDocumentoCodigo { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string CuilCuit { get; set; }
        public string RazonSocial { get; set; }
        public string ProvinciaId { get; set; }
        public string ProvinciaCodigo { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public string CodigoPostal { get; set; }
        public string Calle { get; set; }
        public string CalleNro { get; set; }
        public string PisoDpto { get; set; }
        public string OtrasReferencias { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public decimal Total { get; set; }
        public decimal TotalLista { get; set; }
        public decimal TotalSinImpuesto { get; set; }
        public decimal TotalListaSinImpuesto { get; set; }
        public decimal TotalSinDescuento { get; set; }
        public decimal TotalListaSinDescuento { get; set; }
        public decimal TotalSinImpuestoSinDescuento { get; set; }
        public decimal TotalListaSinImpuestoSinDescuento { get; set; }
        public decimal DescuentoPorcentaje { get; set; }
        public decimal DescuentoTotal { get; set; }
        public decimal DescuentoTotalLista { get; set; }
        public decimal DescuentoSinImpuesto { get; set; }
        public decimal DescuentoListaSinImpuesto { get; set; }
        public decimal ImporteTributos { get; set; }
        public string Observaciones { get; set; }
        public bool Anulado { get; set; }
        public DateTime FechaAnulacion { get; set; }
        public string TipoComprobanteAnulaId { get; set; }
        public string TipoComprobanteAnulaCodigo { get; set; }
        public string TipoComprobanteAnula { get; set; }
        public string LetraAnula { get; set; }
        public int PtoVtaAnula { get; set; }
        public decimal NumeroAnula { get; set; }
        public string CodigoAnula { get; set; }
        public string UsuarioAnula { get; set; }
        public DateTime FechaAlta { get; set; }
        public string UsuarioAlta { get; set; }
        public bool Estado { get; set; }

        public decimal Saldo { get; set; }

        public string ComprobanteId { get; set; }

        public string SucursalNombre { get; set; }
        public string SucursalCalle { get; set; }
        public string SucursalCalleNro { get; set; }
        public string SucursalLocalidad { get; set; }
        public string SucursalCodigoPostal { get; set; }
        public string SucursalTelefono { get; set; }

        public string TipoComprobanteFiscal { get; set; }
        public string LetraFiscal { get; set; }
        public int PtoVentaFiscal { get; set; }
        public decimal NumeroFiscal { get; set; }
    }

    public partial class ComprobantesReciboDTO 
    {
        public string ClienteId { get; set; }
        public string ComprobanteId { get; set; }
        public decimal Importe { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }        

    }

    public partial class ComprobantesEfectivoDTO
    {
        public string ComprobanteId { get; set; }
        public string ClienteId { get; set; }
        public string TipoComprobanteId { get; set; }
        public string FormaPagoId { get; set; }
       
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        public decimal Importe { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }

        public decimal Descuento { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El Saldo debe ser mayor a Cero")]
        public decimal Saldo { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SaldoConDescuento { get; set; }
    }

    public partial class ComprobantesTarjetaDTO
    {
        public string ComprobanteId { get; set; }
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
        [Display(Name = "Monto de Interes")]
        public decimal Interes { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]               
        public decimal Total { get; set; }
        public string TarjetaId { get; set; }
        public string TarjetaNombre { get; set; }
        
        [Display(Name = "Nombre del Cliente")]
        public string TarjetaCliente { get; set; }
        
        [Display(Name = "Numero de Tarjeta")]
        public string TarjetaNumero { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Mes Vence")]
        public string TarjetaVenceMes { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Año Vence")]
        public string TarjetaVenceAño { get; set; }
        [Display(Name = "Codigo Seguridad")]
        public string TarjetaCodigoSeguridad { get; set; }
        public bool TarjetaEsDebito { get; set; }
        [Display(Name = "Codigo de Autorizacion")]
        public string CodigoAutorizacion { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }

        public decimal Descuento { get; set; }
        public decimal Recargo { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El Saldo debe ser mayor a Cero")]
        public decimal Saldo { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SaldoConDescuento { get; set; }
    }

    public partial class ComprobantesChequeDTO
    {
        public string ComprobanteId { get; set; }
        public string ClienteId { get; set; }
        public string FormaPagoId { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        public decimal Importe { get; set; }
        public string ChequeBancoId { get; set; }
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
        public string Observaciones { get; set; }
        public string Usuario { get; set; }
        public decimal Recargo { get; set; }
        public decimal Descuento { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El Saldo debe ser mayor a Cero")]
        public decimal Saldo { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SaldoConDescuento { get; set; }
    }

    public partial class ComprobantesOtroDTO
    {
        public string ComprobanteId { get; set; }
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

        public decimal Descuento { get; set; }
        public decimal Recargo { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999, ErrorMessage = "El Saldo debe ser mayor a Cero")]
        public decimal Saldo { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SaldoConDescuento { get; set; }

    }

    public partial class ComprobantesDolarDTO
    {
        public string ComprobanteId { get; set; }
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

    public partial class ComprobantesFormasPagosDTO
    {
        public string Id { get; set; }        
        public string ClienteId { get; set; }
        public string ComprobanteId { get; set; }
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

    public partial class ComprobantesDTO : IEntidades
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Nro Comprobante")]
        public string Codigo { get; set; }
        public string TipoComprobanteId { get; set; }

        [Display(Name = "Tipo Comprobante")]
        public string TipoComprobante { get; set; }
        public string TipoComprobanteCodigo { get; set; }
        public string PresupuestoId { get; set; }
        public string Letra { get; set; }
        public int PtoVenta { get; set; }
        public decimal Numero { get; set; }

        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaComprobante { get; set; }
        public string ConceptoIncluidoId { get; set; }
        public string ConceptoIncluidoCodigo { get; set; }
        public string ConceptoIncluido { get; set; }
        public DateTime? PeriodoFacturadoDesde { get; set; }
        public DateTime? PeriodoFacturadoHasta { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string TipoResponsableId { get; set; }
        public string TipoResponsableCodigo { get; set; }
        public string TipoResponsable { get; set; }
        public string ClienteId { get; set; }
        public string ClienteCodigo { get; set; }
        public string TipoDocumentoId { get; set; }
        public string TipoDocumentoCodigo { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string CuilCuit { get; set; }
        [Display(Name = "Razon Social")]
        public string RazonSocial { get; set; }
        public string ProvinciaId { get; set; }
        public string ProvinciaCodigo { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public string CodigoPostal { get; set; }
        public string Calle { get; set; }
        public string CalleNro { get; set; }
        public string PisoDpto { get; set; }
        public string OtrasReferencias { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public decimal Total { get; set; }
        public decimal TotalSinImpuesto { get; set; }
        public decimal TotalSinDescuento { get; set; }
        public decimal TotalSinImpuestoSinDescuento { get; set; }
        public decimal DescuentoPorcentaje { get; set; }
        public decimal DescuentoTotal { get; set; }
        public decimal DescuentoSinImpuesto { get; set; }
        public decimal ImporteTributos { get; set; }
        public string Observaciones { get; set; }
        public bool Anulado { get; set; }        
        public DateTime FechaAnulacion { get; set; }
        public string TipoComprobanteAnulaId { get; set; }
        public string TipoComprobanteAnulaCodigo { get; set; }
        public string TipoComprobanteAnula { get; set; }
        public string LetraAnula { get; set; }
        public int PtoVtaAnula { get; set; }
        public decimal NumeroAnula { get; set; }
        public string CodigoAnula { get; set; }
        public string UsuarioAnula { get; set; }
        [Display(Name = "Fecha Alta")]
        public DateTime FechaAlta { get; set; }
        [Display(Name = "Usuario Alta")]
        public string UsuarioAlta { get; set; }
        public bool Estado { get; set; }
        public decimal Saldo { get; set; }

        [Display(Name = "Saldado %")]
        public decimal Saldo_Porcentaje { get; set; }

        [Display(Name = "Codigo")]
        public string CodigoPresupuesto { get; set; }

        public string TipoComprobanteFiscal { get; set; }
        public string LetraFiscal { get; set; }
        public int PtoVentaFiscal { get; set; }
        public decimal NumeroFiscal { get; set; }

        public string SucursalNombre { get; set; }
        public string SucursalCalle { get; set; }
        public string SucursalCalleNro { get; set; }
        public string SucursalLocalidad { get; set; }
        public string SucursalCodigoPostal { get; set; }
        public string SucursalTelefono { get; set; }
    }

    public partial class ComprobantesDetalleDTO : IEntidades
    {
        public string Id { get; set; }
        public string ComprobanteId { get; set; }
        public string ProductoId { get; set; }
        public string ProductoCodigo { get; set; }
        public string ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioSinIva { get; set; }
        public DateTime FechaAlta { get; set; }
        public string UsuarioAlta { get; set; }
    }

    public partial class ComprobantesDetalleImputaDTO : IEntidades
    {
        public string Id { get; set; }
        public string ComprobanteId { get; set; }
        public string DetalleId { get; set; }
        public string ProductoId { get; set; }
        public string ProductoCodigo { get; set; }
        public string ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Precio { get; set; }
        public decimal Imputado { get; set; }
        public decimal ImputadoPorcentaje { get; set; }
        public decimal Importe { get; set; }
        public bool Estado { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }

        public bool EntregaEstado { get; set; }
        public DateTime EntregaFecha { get; set; }
        public string EntregaUsuario { get; set; }

        public bool AutorizaEstado { get; set; }
        public DateTime AutorizaFecha { get; set; }
        public string AutorizaUsuario { get; set; }

        public bool DespachaEstado { get; set; }
        public DateTime DespachaFecha { get; set; }
        public string DespachaUsuario { get; set; }

        public bool DevolucionEstado { get; set; }
        public DateTime DevolucionFecha { get; set; }
        public string DevolucionUsuario { get; set; }
        public string DevolucionMotivo { get; set; }

        public decimal Porcentaje_Config { get; set; }
        
    }

    public partial class ComprobantesDetalleIndicador : IEntidades
    {
        public string Id { get; set; }
        public string ComprobanteId { get; set; }
        public string DetalleId { get; set; }
        public string ProductoId { get; set; }
        public string ProductoCodigo { get; set; }
        public string ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Precio { get; set; }
        public decimal Imputado { get; set; }
        public decimal ImputadoPorcentaje { get; set; }
        public decimal Importe { get; set; }
        public bool Estado { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }

        public bool EntregaEstado { get; set; }
        public DateTime EntregaFecha { get; set; }
        public string EntregaUsuario { get; set; }

        public bool AutorizaEstado { get; set; }
        public DateTime AutorizaFecha { get; set; }
        public string AutorizaUsuario { get; set; }

        public bool DespachaEstado { get; set; }
        public DateTime DespachaFecha { get; set; }
        public string DespachaUsuario { get; set; }

        public bool DevolucionEstado { get; set; }
        public DateTime DevolucionFecha { get; set; }
        public string DevolucionUsuario { get; set; }
        public string DevolucionMotivo { get; set; }

        public string CodigoComprobante { get; set; }
        public DateTime FechaComprobante { get; set; }
        public string UsuarioComprobante { get; set; }
        public string ClienteCodigo { get; set; }
        public string RazonSocial { get; set; }
        public string CodigoPrespuesto { get; set; }

    }
}
