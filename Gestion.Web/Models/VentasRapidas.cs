using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Gestion.Web.Models
{
    public partial class VentasRapidas : IEntidades
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

    public partial class VentasRapidasIndex
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

        [Display(Name = "Fecha Aprobacion")]
        public DateTime? FechaAprobacion { get; set; }

        [Display(Name = "Fecha Rechazo")]
        public DateTime? FechaRechazo { get; set; }

        [Display(Name = "Motivo")]
        public string MotivoAprobacionRechazo { get; set; }

        public string UsuarioAprobacionRechazo { get; set; }
    }

    public partial class VentasRapidasDTO : IEntidades
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha")]
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
        
        [Display(Name = "Cant. Productos")] 
        public decimal CantidadProductos { get; set; }

        [Display(Name = "Precio sin Descuento")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal PrecioSinDescuento { get; set; }

        [Display(Name = "Saldo a Pagar")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Saldo { get; set; }

        public string ComprobanteId { get; set; }
        public string TipoComprobante { get; set; }
        public string CodigoComprobante { get; set; }
        public string TipoComprobanteFiscal { get; set; }
        public string LetraFiscal { get; set; }
        public int PtoVentaFiscal { get; set; }
        public decimal NumeroFiscal { get; set; }
    }

    public partial class VentasRapidasDetalleDTO
    {
        //campos del prespuestos
        public string Id { get; set; }
        public string VentaRapidaId { get; set; }
        public string ProductoId { get; set; }
        public string ProductoCodigo { get; set; }

        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ProductoNombre { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, 9999999, ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }
        public decimal PrecioSinImpuesto { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:N0}")]        
        [Display(Name = "Cantidad de Productos")]
        [Range(1, 100, ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        public int Cantidad { get; set; }
        public DateTime FechaAlta { get; set; }
        public string UsuarioAlta { get; set; }
        public DateTime FechaEdit { get; set; }
        public string UsuarioEdit { get; set; }

        
    }

    public partial class VentasRapidasImprimirDTO
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

        [DisplayFormat(DataFormatString = "{0:C2}")]
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


    public partial class VentasRapidasResumen
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
}
