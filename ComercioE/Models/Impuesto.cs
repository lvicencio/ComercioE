using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class Impuesto
    {
        public int ImpuestoId { get; set; }
        [Display(Name = "Impuesto")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Index("Impuesto_CompaniaId_Descripcion_Index", 2, IsUnique = true)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        [Range(0, 1,ErrorMessage = "Debe seleccionar un {0} entre {1} y {2} ")]
        public double Valor { get; set; }

        [Display(Name = "Compañia")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una Provincia")]
        [Index("Impuesto_CompaniaId_Descripcion_Index", 1, IsUnique = true)]
        public int CompaniaId { get; set; }

        public virtual Compania Compania { get; set; }
    }
}