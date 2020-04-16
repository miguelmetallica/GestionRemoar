using System;
using System.Collections.Generic;

namespace Gestion.Web.Models
{
    public partial class ProductosImagenes : IEntidades
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string ImagenUrl { get; set; }
        public string NombreImagen { get; set; }
    }
}
