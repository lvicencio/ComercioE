using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class CompraDetalle
    {
        [Key]
        public int CompraDetalleId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int CompraId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Display(Name = "Producto")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El{0} debe estar entre {1} y {2}")]
        [Display(Name = "Impuesto")]
        public double Impuesto { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "El valor debe estar {0} entre {1} y {2}")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "El valor debe estar {0} entre {1} y {2}")]
        public double Cantidad { get; set; }


        public virtual Producto Producto { get; set; }

        public virtual Compra Compra { get; set; }
    }
}