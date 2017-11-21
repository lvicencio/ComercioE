using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class Estado
    {
        [Key]
        public int EstadoId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Display(Name = "Estado")]
        [Index("Estado_Descripcion_Index", IsUnique = true)]
        public string Descripcion { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Compra> Compras { get; set; }

    }
}