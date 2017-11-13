using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }

        //[Required(ErrorMessage = "El campo {0} es requerido")]
        //[Range(1, double.MaxValue, ErrorMessage = "Seleccione una Compañia")]
        //[Display(Name = "Compania")]
        //public int CompaniaId { get; set; }

        //UserName=Correo
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(256, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Display(Name = "E-Mail")]
       // [Index("Cliente_UserName_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una Provincia")]
        [Display(Name = "Provincia")]
        public int ProvinciaId { get; set; }

        [Required(ErrorMessage = "TEl campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione una Ciudad")]
        [Display(Name = "Ciudad")]
        public int CiudadId { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName { get { return string.Format("{0} {1}", Nombre, Apellido); } }

        public virtual Provincia Provincia { get; set; }

        public virtual Ciudad Ciudad { get; set; }

        //public virtual Compania Compañia { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<CompaniaCliente> CompaniaClientes { get; set; }

    }
}