using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion.Web.Models
{
    public partial class Proveedores : IEntidades
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        public string RazonSocial { get; set; }
        public string Cuit { get; set; }
        public bool Estado { get; set; }
    }

    public partial class ProveedoresDTO : IEntidades
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        public string RazonSocial { get; set; }
        public string Cuit { get; set; }
        public bool Estado { get; set; }
    }
}
