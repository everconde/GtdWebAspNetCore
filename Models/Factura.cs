using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GtdWebAspNetCore.Models
{
    public class Factura
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string NumeroFactura { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Now.Date;

        [Required]
        [MaxLength(500)]
        public string TipodePago { get; set; } = " ";

        [Required]
        [MaxLength(15)]
        public string DocumentoCliente { get; set; }

        [Required]
        [MaxLength(15)]
        public string NombreCliente { get; set; }

        [Column(TypeName = "money")]        
        public decimal Subtotal { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public decimal Descuento { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public decimal Iva { get; set; }

        [Column(TypeName = "money")]        
        public decimal TotalDescuento { get; set; }

        [Column(TypeName = "money")]        
        public decimal TotalImpuesto { get; set; }

        [Column(TypeName = "money")]        
        public decimal Total { get; set; }

        public virtual List<Detalle> Detalles { get; set; } = new List<Detalle>();

    }
}
