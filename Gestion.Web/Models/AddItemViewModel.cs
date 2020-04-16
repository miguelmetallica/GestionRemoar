using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Web.Models
{
    public class AddItemViewModel
    {
        [Display(Name = "Product")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a product.")]
        public int ProductoId { get; set; }

        [Range(0.0001,int.MaxValue, ErrorMessage = "The quantiy must be a positive number")]
        public int Cantidad { get; set; }

        public IEnumerable<SelectListItem> Productos { get; set; }

    }
}
