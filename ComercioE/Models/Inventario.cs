using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class Inventario
    {
        [Key]
        public int InventarioId { get; set; }

        [Required]
        public int BodegaId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }

        public virtual Bodega Bodega { get; set; }

        public virtual Producto Producto { get; set; }

       
    }
}