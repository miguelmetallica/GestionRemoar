using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion.Web.Models
{
    public class Usuarios : IdentityUser
    {
        [Display(Name = "First Name")]
	    public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Address { get; set; }

        public string SucursalId { get; set; }

        public Sucursales Sucursal { get; set; }

        [Display(Name = "Phone Number")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        [Display(Name = "Full Name")]
        public string FullName { get { return $"{this.FirstName} {this.LastName}"; } }

        [Display(Name = "Email Confirmed")]
        public override bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }

        [NotMapped]
        [Display(Name = "Is Admin?")]
        public bool IsAdmin { get; set; }


    }

    public class RolesDto:IEntidades
    {        
        public string Id { get; set; }        
        public string UserId { get; set; }
        [Display(Name = "Rol")]
        public string Name { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }


    }
}
