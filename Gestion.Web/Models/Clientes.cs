using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion.Web.Models
{
    public partial class Clientes : IEntidades
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string RazonSocial { get; set; }
        public string TipoDocumentoId { get; set; }
        public string NroDocumento { get; set; }
        public string CuilCuit { get; set; }
        public bool esPersonaJuridica { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string ProvinciaId { get; set; }
        public string Localidad { get; set; }
        public string CodigoPostal { get; set; }
        public string Calle { get; set; }
        public string CalleNro { get; set; }
        public string PisoDpto { get; set; }
        public string OtrasReferencias { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string TipoResponsableId { get; set; }
        public string CategoriaId { get; set; }
        public bool Estado { get; set; }
    }

    public partial class ClientesDTO : IEntidades
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string RazonSocial { get; set; }
        public string TipoDocumentoId { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string CuilCuit { get; set; }
        public bool esPersonaJuridica { get; set; }

        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; }
        public string ProvinciaId { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public string CodigoPostal { get; set; }
        public string Calle { get; set; }
        public string CalleNro { get; set; }
        public string PisoDpto { get; set; }
        public string OtrasReferencias { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string TipoResponsableId { get; set; }
        public string TipoResponsable { get; set; }
        public string CategoriaId { get; set; }
        public string Categoria { get; set; }
        public bool Estado { get; set; }
    }
}
