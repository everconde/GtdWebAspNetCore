using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GtdWebAspNetCore.Models
{
    public class Detalle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Factura")]
        public int FacturaId { get; set; }
        public virtual Factura Factura { get; private set; }

        [Required]
        [ForeignKey("Producto")]
        [MaxLength(6)]
        public string ProductoID { get; set; }

        public virtual Producto Producto { get; private set; }

        [Column(TypeName = "money")]
        [Required]
        [Range(1, 1000, ErrorMessage = "La cantidad debe ser superior a 0")]
        public decimal Cantidad { get; set; }
        
        [Range(1, 10000000, ErrorMessage = "Precio debe ser mayor que 0")]
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "money")]
        [Required]
        public decimal PrecioUnitario { get; set; }
        
        [NotMapped]
        public bool IsDeleted { get; set; } = false;
    }
}
