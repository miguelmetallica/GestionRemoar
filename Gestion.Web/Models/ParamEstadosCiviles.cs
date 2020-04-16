﻿using System.ComponentModel.DataAnnotations;

namespace Gestion.Web.Models
{
    public partial class ParamEstadosCiviles : IEntidades
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(6)]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150)]
        public string Descripcion { get; set; }

        public bool Estado { get; set; }

        public Usuarios User { get; set; }
    }
}