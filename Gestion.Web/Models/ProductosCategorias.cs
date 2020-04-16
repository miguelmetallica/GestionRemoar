using System;
using System.Collections.Generic;

namespace Gestion.Web.Models
{
    public partial class ProductosCategorias : IEntidades
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int CategoriaId { get; set; }
    }
}
