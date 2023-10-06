using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GtdWebAspNetCore.Models
{
    public class Producto
    {
        //[Remote("IsProductCodeValid", "Producto", AdditionalFields = "Name", ErrorMessage = "Codigo del producto ya existe")]
        [Key]
        [StringLength(6)]
        public string ID { get; set; }

        //[Remote("IsProductNameValid", "Producto", AdditionalFields = "Code", ErrorMessage = "Nombre del producto ya existe")]
        [Required]
        [StringLength(75)]
        public String NombreProducto { get; set; }
        
    }
}
