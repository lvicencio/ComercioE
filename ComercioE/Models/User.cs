using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        //el username sera el correo en la tabla aspuser
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(256, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Index("User_UserName_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        public string Nombre { get; set; }

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        public string Apellido { get; set; }

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
        public string Foto { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una {0}")]
        [Display(Name = "Provincia")]
        public int ProvinciaId { get; set; }
       
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una {0}")]
        [Display(Name = "Ciudad")]
        public int CiudadId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una {0}")]
        [Display(Name = "Compañia")]
        public int CompaniaId { get; set; }

        //propiedad de lectura, no lleva set, y debe retornar un valor,
        //en este caso el nombre completo
        [Display(Name = "Nombre Completo")]
        public string FullName { get { return string.Format("{0}{1}", Nombre, Apellido); } }


        [NotMapped]
        public HttpPostedFileBase FotoFile { get; set; }
        //relaciones
        //una compañia pertenece a una ciudad
        public virtual Ciudad Ciudad { get; set; }
        public virtual Provincia Provincia { get; set; }
        public virtual Compania Compañia { get; set; }
    }
}