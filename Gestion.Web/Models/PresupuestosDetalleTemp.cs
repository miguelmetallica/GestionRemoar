﻿using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class PresupuestosDetalleTemp : IEntidades
    {
        public string Id { get; set; }

        [Required]
        public Usuarios User { get; set; }

        [Required]
        public Productos Producto { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Precio { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public int Cantidad { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal SubTotal { get { return this.Precio * (decimal)this.Cantidad; } }

    }
}
