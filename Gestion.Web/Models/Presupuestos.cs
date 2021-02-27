using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Gestion.Web.Models
{
    public partial class Presupuestos : IEntidades
    {
        public string Id { get; set; }

        public string Codigo { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha Presupuesto")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha Vencimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaVencimiento { get; set; }

        public string ClienteId { get; set; }
        public Clientes Cliente { get; set; }
        
        public string EstadoId { get; set; }
        public ParamPresupuestosEstados Estado { get; set; }

        public DateTime FechaAlta { get; set; }
        public string UsuarioAlta { get; set; }
        
        public IEnumerable<PresupuestosDetalle> Item { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Lines { get { return this.Item == null ? 0 : this.Item.Count(); } }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public double Cantidad { get { return this.Item == null ? 0 : this.Item.Sum(i => i.Cantidad); } }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Total { get { return this.Item == null ? 0 : this.Item.Sum(i => i.SubTotal); } }

        [Display(Name = "Fecha Presupuesto")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public DateTime? FechaLocal
        {
            get
            {
                if (this.Fecha == null)
                {
                    return null;
                }

                return this.Fecha.ToLocalTime();
            }
        }

        [Display(Name = "Fecha Aprobacion")]
        public DateTime? FechaAprobacion { get; set; }

        [Display(Name = "Fecha Rechazo")]
        public DateTime? FechaRechazo { get; set; }

        [Display(Name = "Motivo")]
        public string MotivoAprobacionRechazo { get; set; }

        public string UsuarioAprobacionRechazo { get; set; }
    }

    public partial class PresupuestosIndex
    {
        public string Id { get; set; }

        public string Codigo { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Display(Name = "Vence")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaVencimiento { get; set; }
        [Display(Name = "Cliente")]
        public string RazonSocial { get; set; }
        [Display(Name = "Documento")]
        public string NroDocumento { get; set; }
        [Display(Name = "Cuil/Cuit")]
        public string CuilCuit { get; set; }
        public string Estado { get; set; }
        [Display(Name = "Usuario")]
        public string UsuarioAlta { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Cantidad { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Total { get; set; }

        //[DisplayFormat(DataFormatString = "{0:C2}")]
        //public decimal TotalContado { get; set; }

        [Display(Name = "Fecha Aprobacion")]
        public DateTime? FechaAprobacion { get; set; }

        [Display(Name = "Fecha Rechazo")]
        public DateTime? FechaRechazo { get; set; }

        [Display(Name = "Motivo")]
        public string MotivoAprobacionRechazo { get; set; }

        public string UsuarioAprobacionRechazo { get; set; }
    }

    public partial class PresupuestosDTO : IEntidades
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha Presupuesto")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha Vencimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaVencimiento { get; set; }
        public string ClienteId { get; set; }
        [Display(Name = "Codigo de Cliente")]
        public string ClienteCodigo { get; set; }
        [Display(Name = "Razon Social")]
        public string ClienteRazonSocial { get; set; }
        [Display(Name = "Nro Documento")]
        public string ClienteNroDocumento { get; set; }
        [Display(Name = "CUIT/CUIL")]
        public string ClienteCuitCuil { get; set; }
        public string TipoResponsableId { get; set; }
        [Display(Name = "Tipo Responsable")]
        public string TipoResponsable { get; set; }
        public string ClienteCategoriaId { get; set; }
        public string ClienteCategoria { get; set; }
        public string EstadoId { get; set; }
        public string Estado { get; set; }
        public string DescuentoId { get; set; }
        public string Descuento { get; set; }
        public decimal DescuentoPorcentaje { get; set; }
        [Display(Name = "Fecha Aprobacion")]
        public DateTime? FechaAprobacion { get; set; }
        [Display(Name = "Fecha Rechazo")]
        public DateTime? FechaRechazo { get; set; }
        [Display(Name = "Motivo")]
        public string MotivoAprobacionRechazo { get; set; }
        public string UsuarioAprobacionRechazo { get; set; }
        public DateTime FechaAlta { get; set; }
        [Display(Name = "Usuario Alta")]
        public string UsuarioAlta { get; set; }
        public DateTime FechaEdit { get; set; }
        public string UsuarioEdit { get; set; }

        [Display(Name = "Precio")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Precio { get; set; }
        //[Display(Name = "Precio Contado")]
        //[DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal PrecioContado { get; set; }

        [Display(Name = "Cant. Productos")] 
        public decimal CantidadProductos { get; set; }
    }

    public partial class PresupuestosDetalleDTO
    {
        //campos del prespuestos
        public string Id { get; set; }
        public string PresupuestoId { get; set; }
        public string ProductoId { get; set; }
        public string ProductoCodigo { get; set; }
        public string ProductoNombre { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        public decimal PrecioSinImpuesto { get; set; }    
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Cantidad de Productos")]
        public int Cantidad { get; set; }
        public DateTime FechaAlta { get; set; }
        public string UsuarioAlta { get; set; }
        public DateTime FechaEdit { get; set; }
        public string UsuarioEdit { get; set; }

        
    }

    public partial class PresupuestosImprimirDTO
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha Presupuesto")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha Vencimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaVencimiento { get; set; }
        public string ClienteId { get; set; }
        public string ClienteCodigo { get; set; }
        public string ClienteRazonSocial { get; set; }
        public string ClienteNroDocumento { get; set; }
        public string ClienteCuitCuil { get; set; }
        public string TipoResponsableId { get; set; }
        public string TipoResponsable { get; set; }
        public string ClienteCategoriaId { get; set; }
        public string ClienteCategoria { get; set; }
        public string EstadoId { get; set; }
        public string Estado { get; set; }
        public string DescuentoId { get; set; }
        public string Descuento { get; set; }
        public decimal DescuentoPorcentaje { get; set; }
        [Display(Name = "Fecha Aprobacion")]
        public DateTime? FechaAprobacion { get; set; }
        [Display(Name = "Fecha Rechazo")]
        public DateTime? FechaRechazo { get; set; }
        [Display(Name = "Motivo")]
        public string MotivoAprobacionRechazo { get; set; }
        public string UsuarioAprobacionRechazo { get; set; }
        public DateTime FechaAlta { get; set; }
        public string UsuarioAlta { get; set; }
        public DateTime FechaEdit { get; set; }
        public string UsuarioEdit { get; set; }

        public decimal Precio { get; set; }
        public decimal CantidadProductos { get; set; }

        public string SucursalNombre { get; set; }
        public string SucursalCalle { get; set; }
        public string SucursalCalleNro { get; set; }
        public string SucursalLocalidad { get; set; }
        public string SucursalCodigoPostal { get; set; }
        public string SucursalTelefono { get; set; }

        public string VersionImpresion { get; set; }
    }

    public partial class PresupuestosResumen
    {
        public string Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Cantidad de Productos")]
        public int CantidadProductos { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "SubTotal")]
        public decimal SubTotalProductos { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Descuento %")]
        public decimal DescuentoPorcentaje { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Descuento $")]
        public decimal DescuentoMonto { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Total a Pagar")]
        public decimal TotalAPagar { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Saldo a Pagar")]
        public decimal SaldoAPagar { get; set; }

    }
    public partial class PresupuestosFormasPagosDTO
    {
        public string Id { get; set; }
        public string ClienteId { get; set; }
        public string PresupuestoId { get; set; }
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
}
