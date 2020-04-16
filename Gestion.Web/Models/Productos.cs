using System;
using System.Collections.Generic;

namespace Gestion.Web.Models
{
    public partial class Productos : IEntidades
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Tipo { get; set; }
        public decimal? Precio { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionCorta { get; set; }
        public IEnumerable<ProductosCategorias> ProductosCategorias { get; set; }
        public IEnumerable<ProductosEtiquetas> ProductosEtiquetas { get; set; }
        public IEnumerable<ProductosImagenes> ProductosImagenes { get; set; }
    }
}
