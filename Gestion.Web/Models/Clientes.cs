using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion.Web.Models
{
    public partial class Clientes : IEntidades
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(10)]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(130)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(130)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(130)]
        public string RazonSocial { get; set; }

        public ParamTiposDocumentos TipoDocumento { get; set; }

        public string NroDocumento { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public ParamEstadosCiviles EstadoCivil { get; set; }

        public ParamNacionalidades Nacionalidad { get; set; }

        public bool PersonaJuridica { get; set; }

        public ParamProvincias Provincia { get; set; }

        public ParamLocalidades Localidad { get; set; }

        public string Calle { get; set; }

        public string CalleNumero { get; set; }

        public string CalleReferencias { get; set; }

        public string CodigoPostal { get; set; }

        public string Telefono { get; set; }

        public string Celular { get; set; }

        public string Email { get; set; }

        public ParamCondicionesIva CondicionIva { get; set; }

        public bool Estado { get; set; }

        public Usuarios User { get; set; }
    }
}
