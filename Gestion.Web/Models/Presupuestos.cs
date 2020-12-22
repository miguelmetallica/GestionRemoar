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

        [Display(Name = "Fecha Aprobacion")]
        public DateTime? FechaAprobacion { get; set; }

        [Display(Name = "Fecha Rechazo")]
        public DateTime? FechaRechazo { get; set; }

        [Display(Name = "Motivo")]
        public string MotivoAprobacionRechazo { get; set; }

        public string UsuarioAprobacionRechazo { get; set; }
    }

    public partial class PresupuestosDetalleDTO
    {
        //campos del prespuestos
        public string Id { get; set; }
        public string DetalleId { get; set; }
        public string Codigo { get; set; }
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        [Display(Name = "Vence")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaVencimiento { get; set; }
        public string Estado { get; set; }
        [Display(Name = "Descuento")]
        public decimal Descuento { get; set; }
        [Display(Name = "Usuario")]
        public string UsuarioAlta { get; set; }

        //campos del cliente
        [Display(Name = "ClienteId")]
        public string ClienteId { get; set; }
        [Display(Name = "Codigo Cliente")]
        public string ClienteCodigo { get; set; }
        
        [Display(Name = "Cliente")]
        public string ClienteRazonSocial { get; set; }
        [Display(Name = "Documento")]
        public string ClienteNroDocumento { get; set; }
        [Display(Name = "Cuil/Cuit")]
        public string ClienteCuilCuit { get; set; }
        [Display(Name = "Tipo Responsable")]
        public string TipoResponsableId { get; set; }
        [Display(Name = "Tipo Responsable")]
        public string TipoResponsable { get; set; }
        //campos totales
        //cantidad total de productos
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Cantidad de Productos")]
        public int Cantidad { get; set; }

        //total de la venta
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "SubTotal")]
        public decimal Total { get; set; }

        //total de la venta
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Total")]
        public decimal TotalDescuento { get; set; }

        //total sin impuestos de la venta
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "SubTotal Sin Impuestos")]
        public decimal TotalSinImpuesto { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Total Sin Impuestos")]
        public decimal TotalSinImpuestoDescuento { get; set; }

        //subtotal por producto
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SubTotal { get; set; }

        //subtotal sin impuestos por producto
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SubTotalSinImpuesto { get; set; }
        
        
        //campos del productos
        [Display(Name = "ProductoId")]
        public string ProductoId { get; set; }
        [Display(Name = "Codigo")]
        public string ProductoCodigo { get; set; }
        [Display(Name = "Producto")]
        public string ProductoNombre { get; set; }
        [Display(Name = "Precio")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal ProductoPrecio { get; set; }
        [Display(Name = "Precio Sin Imp")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal ProductoPrecioSinImpuesto { get; set; }
        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int ProductoCantidad { get; set; }


        [Display(Name = "Fecha Aprobacion")]
        public DateTime? FechaAprobacion { get; set; }

        [Display(Name = "Fecha Rechazo")]
        public DateTime? FechaRechazo { get; set; }

        [Display(Name = "Motivo")]
        public string MotivoAprobacionRechazo { get; set; }

        public string UsuarioAprobacionRechazo { get; set; }

        public string SucursalNombre { get; set; }
        public string SucursalCalle { get; set; }
        public string SucursalCalleNro { get; set; }
        public string SucursalLocalidad { get; set; }
        public string SucursalCodigoPostal { get; set; }
        public string SucursalTelefono { get; set; }

        public string VersionImpresion { get; set; }
    }
}
