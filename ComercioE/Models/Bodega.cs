using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class Bodega
    {
        [Key]
        public int BodegaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una {0}")]
        [Index("Bodega_CompaniaId_Nombre_Index", 1, IsUnique = true)]
        [Display(Name = "Compañia")]
        public int CompaniaId { get; set; }

        [Display(Name = "Bodega")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Index("Bodega_CompaniaId_Nombre_Index", 2,IsUnique = true)]
        public string Nombre { get; set; }

        [Display(Name = "Telefono")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Display(Name = "Direccion")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una {0}")]
        [Display(Name = "Provincia")]
        public int ProvinciaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una {0}")]
        [Display(Name = "Ciudad")]
        public int CiudadId { get; set; }

         //relaciones
        //una compañia pertenece a una ciudad
        public virtual Ciudad Ciudad { get; set; }
        public virtual Provincia Provincia { get; set; }
        public virtual Compania Compañia { get; set; }

        public virtual ICollection<Inventario> Inventarios { get; set; }
    }
}