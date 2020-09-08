using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Gestion.Web.Models
{
    public partial class ProductosImagenes : IEntidades
    {
        public string Id { get; set; }
        public string ProductoId { get; set; }
        public Productos Producto { get; set; }
        public string ImagenUrl { get; set; }        
    }

    public partial class ProductosImagenesViewModel : ProductosImagenes
    {
        public IFormFile ImageFile { get; set; }        
    }
}
