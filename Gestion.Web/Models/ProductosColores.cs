﻿using System;
using System.Collections.Generic;

namespace Gestion.Web.Models
{
    public partial class ProductosColores : IEntidades
    {
        public string Id { get; set; }
        public string ProductoId { get; set; }
        public string CategoriaId { get; set; }
    }
}
