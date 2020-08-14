using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Gestion.Web.Models
{
    public partial class Presupuestos : IEntidades
    {
        public string Id { get; set; }

        public string Codigo { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha Presupuesto")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es valido")]
        [Display(Name = "Fecha Vencimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaVencimiento { get; set; }

        public string ClienteId { get; set; }
        public Clientes Cliente { get; set; }
        
        public string EstadoId { get; set; }
        public ParamPresupuestosEstados Estado { get; set; }

        public DateTime FechaAlta { get; set; }
        public string UsuarioAlta { get; set; }
        
        public IEnumerable<PresupuestosDetalle> Item { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Lines { get { return this.Item == null ? 0 : this.Item.Count(); } }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Cantidad { get { return this.Item == null ? 0 : this.Item.Sum(i => i.Cantidad); } }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Total { get { return this.Item == null ? 0 : this.Item.Sum(i => i.SubTotal); } }

        [Display(Name = "Fecha Presupuesto")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public DateTime? FechaLocal
        {
            get
            {
                if (this.Fecha == null)
                {
                    return null;
                }

                return this.Fecha.ToLocalTime();
            }
        }

    }
}
