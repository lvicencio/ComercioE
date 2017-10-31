using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class Ciudad
    {
        [Key]
        public int CiudadId { get; set; }
        [Display(Name = "Ciudad")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Index("Ciudad_Nombre_Index", 2, IsUnique = true)]
        public string Nombre { get; set; }
        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una Provincia")]
      [Index("Ciudad_Nombre_Index", 1,IsUnique = true)]
        public int ProvinciaId { get; set; }

        //relaciones
        //una ciudad tiene una provincia, tiene varias compañias, tiene muchos usuarios
        public virtual Provincia Provincia { get; set; }
        public virtual ICollection<Compania> Companias { get; set; }
        public virtual ICollection<User> Usuarios { get; set; }

        public virtual ICollection<Bodega> Bodegas { get; set; }

    }
}