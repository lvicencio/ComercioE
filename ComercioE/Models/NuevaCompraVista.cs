using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class NuevaCompraVista
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione un Cliente")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Seleccione un Cliente")]
        [Display(Name = "Cliente")]
        public int BodegaId{ get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comentarios { get; set; }

        public List<CompraDetalleTmps> Detalles { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double TotalCantidad { get { return Detalles == null ? 0 : Detalles.Sum(d => d.Cantidad); } }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal TotalValor { get { return Detalles == null ? 0 : Detalles.Sum(d => d.Valor); } }

    }
}