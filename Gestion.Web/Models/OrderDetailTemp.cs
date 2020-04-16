using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class OrderDetailTemp : IEntidades
    {
        public int Id { get; set; }

        [Required]
        public Usuarios User { get; set; }

        [Required]
        public Productos Producto { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Precio { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Cantidad { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SubTotal { get { return this.Precio * (decimal)this.Cantidad; } }

    }
}
