using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class Provincia
    {
       [Key]
        public int ProvinciaId { get; set; }
        [Index("Provincia_Nombre_Index", IsUnique = true)]
        [Display(Name = "Provincia")]
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage ="El campo {0} debe tener maximo {1} caracteres")]
        public string Nombre { get; set; }
        
        //relaciones
        //una provincia tiene muchas ciudades
        public virtual ICollection<Ciudad> Ciudades { get; set; }
        public virtual ICollection<Compania> Companias { get; set; }
        public virtual ICollection<User> Usuarios { get; set; }

        public virtual ICollection<Bodega> Bodegas { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}