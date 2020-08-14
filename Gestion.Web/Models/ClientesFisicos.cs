using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion.Web.Models
{
    public partial class ClientesFisicoAdd : IEntidades
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(140, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un {0}")]
        [Display(Name = "Tipo de Documento")]
        public string TipoDocumentoId { get; set; }

        [Required(ErrorMessage = "Debes ingresar un {0}")]
        [StringLength(8, ErrorMessage =
            "El campo {0} debe contener como maximo {1} y un minimo de {2} caracteres",
            MinimumLength = 5)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numero de {0} no valido")]
        [Display(Name = "Nro de Documento")]
        public string NroDocumento { get; set; }

        [Required(ErrorMessage = "Debes ingresar el {0}")]
        [StringLength(20, ErrorMessage =
            "El campo {0} debe contener como maximo {1} y un minimo de {2} caracteres",
            MinimumLength = 11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Solo se permiten números.")]
        [Display(Name = "Cuil")]
        public string CuilCuit { get; set; }

        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha de Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaNacimiento { get; set; }

        [Display(Name = "Provincia")]
        public string ProvinciaId { get; set; }
        [MaxLength(250, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Localidad { get; set; }
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        [Display(Name = "Codigo Postal")]
        public string CodigoPostal { get; set; }
        [MaxLength(500, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Calle { get; set; }
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        [Display(Name = "Numero")]
        public string CalleNro { get; set; }
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        [Display(Name = "Piso / Departamento")]
        public string PisoDpto { get; set; }
        [MaxLength(500, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        [Display(Name = "Otras Referencias")]
        public string OtrasReferencias { get; set; }
        [MaxLength(15, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "El campo {0} no es valido")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Debes ingresar un {0}")]
        [MaxLength(15, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "El campo {0} no es valido")]
        public string Celular { get; set; }
        
        [MaxLength(150, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El campo {0} no es valido")]
        public string Email { get; set; }
        public bool Estado { get; set; }

        public string UsuarioAlta { get; set; }

        [NotMapped]
        public string ExternalId { get; set; }
    }

    public partial class ClientesFisicoEdit : IEntidades
    {
        [Key]
        public string Id { get; set; }

        public string Codigo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(140, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un {0}")]
        [Display(Name = "Tipo de Documento")]
        public string TipoDocumentoId { get; set; }

        [Required(ErrorMessage = "Debes ingresar un {0}")]
        [StringLength(8, ErrorMessage =
            "El campo {0} debe contener como maximo {1} y un minimo de {2} caracteres",
            MinimumLength = 5)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numero de {0} no valido")]
        [Display(Name = "Nro de Documento")]
        public string NroDocumento { get; set; }

        [Required(ErrorMessage = "Debes ingresar el {0}")]
        [StringLength(20, ErrorMessage =
            "El campo {0} debe contener como maximo {1} y un minimo de {2} caracteres",
            MinimumLength = 11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Solo se permiten números.")]
        [Display(Name = "Cuil")]
        public string CuilCuit { get; set; }

        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha de Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaNacimiento { get; set; }
        public string ProvinciaId { get; set; }
        [MaxLength(250, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Localidad { get; set; }
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string CodigoPostal { get; set; }
        [MaxLength(500, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Calle { get; set; }
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string CalleNro { get; set; }
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string PisoDpto { get; set; }
        [MaxLength(500, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string OtrasReferencias { get; set; }
        [MaxLength(15, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "El campo {0} no es valido")]
        public string Telefono { get; set; }
        [MaxLength(15, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "El campo {0} no es valido")]
        public string Celular { get; set; }
        [MaxLength(150, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El campo {0} no es valido")]
        public string Email { get; set; }
        public bool Estado { get; set; }
        public string UsuarioAlta { get; set; }
    }
}
