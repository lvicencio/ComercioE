using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class CompaniaCliente
    {
        [Key]
        public int CompaniaClienteId { get; set; }
        public int ClienteId { get; set; }
        public int CompaniaId { get; set; }

       

        public virtual Compania Compañia { get; set; }

        public virtual Cliente  Cliente { get; set; }
    }
}