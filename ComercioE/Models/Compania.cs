using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class Compania
    {
        [Key]
        public int CompaniaId { get; set; }
        [Index("Compania_Nombre_Index", IsUnique = true)]
        [Display(Name = "Compañia")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
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

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una Provincia")]
        public int ProvinciaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una Provincia")]
        public int CiudadId { get; set; }

        [NotMapped]
        public HttpPostedFileBase LogoFile { get; set; }
        //relaciones
        //una compañia pertenece a una ciudad
        //compañia tiene muchos usuarios, compañias
        public virtual Ciudad Ciudad { get; set; }
        public virtual Provincia Provincia { get; set; }
        public virtual ICollection<User> Usuarios { get; set; }
        public virtual ICollection<Compania> Companias { get; set; }

        public virtual ICollection<Impuesto> Impuestos{ get; set; }
    }
}