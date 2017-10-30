using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }
        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Index("Categoria_CompaniaId_Descripcion_Index", 2, IsUnique = true)]
        public string Descripcion { get; set; }
        [Display(Name = "Compañia")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una Provincia")]
        [Index("Categoria_CompaniaId_Descripcion_Index", 1, IsUnique = true)]
        public int CompaniaId { get; set; }

        public virtual Compania Compania { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }
}