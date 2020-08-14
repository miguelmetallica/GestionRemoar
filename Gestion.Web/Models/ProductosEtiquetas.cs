using System;
using System.Collections.Generic;

namespace Gestion.Web.Models
{
    public partial class ProductosEtiquetas : IEntidades
    {
        public string Id { get; set; }
        public string ProductoId { get; set; }
        public string EtiquetaId { get; set; }
    }
}
