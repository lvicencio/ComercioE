using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class Compra
    {
        [Key]
        public int CompraId { get; set; }

        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione un Proveedor")]
        public int ProveedorId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una {0}")]
        [Display(Name = "Estado")]
        public int EstadoId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comentarios { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una {0}")]
        [Display(Name = "Bodega")]
        public int BodegaId { get; set; }

        public virtual   Proveedor Proveedor { get; set; }

        public virtual Estado Estado { get; set; }

        public virtual Bodega Bodega { get; set; }

        public virtual Compania Compañia { get; set; }

        public virtual ICollection<CompraDetalle> CompraDetalles { get; set; }
    }
}