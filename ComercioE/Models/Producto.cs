using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }

        [Display(Name = "Compañia")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una Provincia")]
        [Index("Producto_CompaniaId_Descripcion_Index", 1, IsUnique = true)]
        [Index("Producto_CompaniaId_BarCode_Index", 1, IsUnique = true)]
        public int CompaniaId { get; set; }

        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Index("Producto_CompaniaId_Descripcion_Index", 2, IsUnique = true)]
        public string Descripcion { get; set; }

        [Display(Name = "Codigo de Barra")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(13, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Index("Producto_CompaniaId_BarCode_Index", 2, IsUnique = true)]
        public string BarCode { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una Provincia")]
       public int CategoriaId { get; set; }

        [Display(Name = "Impuesto")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una Provincia")]
        public int ImpuestoId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar un {0} entre {1} y {2} ")]
        public decimal Precio { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Imagen { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImagenFile { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comentarios { get; set; }

        public virtual Compania Compania { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual Impuesto Impuesto { get; set; }

    }
}