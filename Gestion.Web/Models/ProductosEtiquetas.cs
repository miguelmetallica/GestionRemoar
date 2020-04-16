using System;
using System.Collections.Generic;

namespace Gestion.Web.Models
{
    public partial class ProductosEtiquetas : IEntidades
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int EtiquetaId { get; set; }
    }
}
