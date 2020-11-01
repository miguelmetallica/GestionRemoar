using System;
using System.ComponentModel.DataAnnotations;

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
        public decimal TotalSinImpuesto { get; set; }
        public decimal TotalSinDescuento { get; set; }
        public decimal TotalSinImpuestoSinDescuento { get; set; }
        public decimal DescuentoPorcentaje { get; set; }
        public decimal DescuentoTotal { get; set; }
        public decimal DescuentoSinImpuesto { get; set; }
        public decimal ImporteTributos { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaAnulacion { get; set; }
        public string TipoComprobanteAnulaId { get; set; }
        public string TipoComprobanteAnulaCodigo { get; set; }
        public string TipoComprobanteAnula { get; set; }
        public string LetraAnula { get; set; }
        public int PtoVtaAnula { get; set; }
        public decimal NumeroAnula { get; set; }
        public DateTime FechaAlta { get; set; }
        public string UsuarioAlta { get; set; }
        public bool Estado { get; set; }

        public decimal Saldo { get; set; }
    }

    public partial class ComprobantesReciboDTO 
    {
        public string ClienteId { get; set; }
        public decimal Importe { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }        

    }

    public partial class ComprobantesEfectivoDTO
    {
        public string ClienteId { get; set; }
        public string FormaPagoId { get; set; }
        public decimal Importe { get; set; }
        public string Usuario { get; set; }

    }

    public partial class ComprobantesTarjetaDTO
    {
        public string ClienteId { get; set; }
        public string FormaPagoId { get; set; }
        public decimal Importe { get; set; }
        public int Cuota { get; set; }
        public decimal Interes { get; set; }
        public decimal Total { get; set; }
        public string TarjetaId { get; set; }
        public string TarjetaNombre { get; set; }
        public string TarjetaCliente { get; set; }
        public string TarjetaNumero { get; set; }
        public string TarjetaVenceMes { get; set; }
        public string TarjetaVenceAño { get; set; }
        public string TarjetaCodigoSeguridad { get; set; }
        public bool TarjetaEsDebito { get; set; }
        public string Usuario { get; set; }
    }

    public partial class ComprobantesChequeDTO
    {
        public string ClienteId { get; set; }
        public string FormaPagoId { get; set; }
        public decimal Importe { get; set; }
        public string ChequeBancoId { get; set; }
        public string ChequeBanco { get; set; }
        public string ChequeNumero { get; set; }
        public DateTime ChequeFechaEmision { get; set; }
        public DateTime ChequeFechaVencimiento { get; set; }
        public string ChequeCuit { get; set; }
        public string ChequeNombre { get; set; }
        public string ChequeCuenta { get; set; }
        public string Usuario { get; set; }
    }

    public partial class ComprobantesOtroDTO
    {
        public string ClienteId { get; set; }
        public decimal Importe { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }

    }
}
