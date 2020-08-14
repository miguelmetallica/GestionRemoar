using System;
using System.Collections.Generic;

namespace Gestion.Web.Models
{
    public partial class Etiquetas : IEntidades
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Slug { get; set; }
        public string Descripcion { get; set; }
    }
}
