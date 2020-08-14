using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion.Web.Models
{
    public partial class Productos : IEntidades
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Codigo { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Producto { get; set; }

        [Display(Name = "Tipo de Producto")]
        public string TipoProductoId { get; set; }

        [Display(Name = "Descripcion")]
        [MaxLength(500, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string DescripcionCorta { get; set; }

        [Display(Name = "Resumen")]
        [MaxLength(4000, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string DescripcionLarga { get; set; }
        
        public decimal? Peso { get; set; }

        [Display(Name = "Longitud")]
        [Range(0,double.MaxValue,ErrorMessage = "The field {0} acepta valores entre {1} y {2}")]
        public decimal? DimencionesLongitud { get; set; }

        [Display(Name = "Ancho")]
        [Range(0, double.MaxValue, ErrorMessage = "The field {0} acepta valores entre {1} y {2}")]
        public decimal? DimencionesAncho { get; set; }

        [Display(Name = "Alto")]
        [Range(0, double.MaxValue, ErrorMessage = "The field {0} acepta valores entre {1} y {2}")]
        public decimal? DimencionesAltura { get; set; }

        [Display(Name = "Precio de Compra")]
        [Range(0, double.MaxValue, ErrorMessage = "The field {0} acepta valores entre {1} y {2}")]
        public decimal? PrecioCompra { get; set; }

        [Display(Name = "Cuenta de Venta")]
        public string CuentaVentaId { get; set; }

        [Display(Name = "Cuenta de Compra")]
        public string CuentaCompraId { get; set; }

        [Display(Name = "Unid. Medida")]
        public string UnidadMedidaId { get; set; }

        [Display(Name = "Marca")]
        public string MarcaId { get; set; }

        public bool Visible { get; set; }

        [Display(Name = "Precio")]
        [Range(0, double.MaxValue, ErrorMessage = "The field {0} acepta valores entre {1} y {2}")]
        public decimal? PrecioNormal { get; set; }

        [Display(Name = "Rebajado")]
        [Range(0, double.MaxValue, ErrorMessage = "The field {0} acepta valores entre {1} y {2}")]
        public decimal? PrecioRebajado { get; set; }

        public bool Estado { get; set; }

        public DateTime FechaAlta { get; set; }
        public string UsuarioAlta { get; set; }

        [NotMapped]
        public string ExternalId { get; set; }
        [NotMapped]
        public bool EsVendedor { get; set; }

        public IEnumerable<ProductosCategorias> ProductosCategorias { get; set; }
        public IEnumerable<ProductosEtiquetas> ProductosEtiquetas { get; set; }
        public IEnumerable<ProductosImagenes> ProductosImagenes { get; set; }
    }
}
