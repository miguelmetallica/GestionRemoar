using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion.Web.Models
{
    public partial class PresupuestosDetalle : IEntidades
    {
        public string Id { get; set; }

        public string PresupuestoId { get; set; }
        public Presupuestos Presupuesto { get; set; }

        public string ProductoId { get; set; }
        public Productos Producto { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, 9999999, ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        public decimal Precio { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1,50,ErrorMessage = "El campo {0} puede tomar valores entre {1} y {2}")]
        public int Cantidad { get; set; }

        public string UsuarioAlta { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SubTotal { get { return this.Precio * (decimal)this.Cantidad; } }

        [NotMapped]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display( Name = "Producto")]
        public string NombreProducto { get; set; }

    }
}
