using GtdWebAspNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GtdWebAspNetCore.Data
{
    public static class DbInitializer
    {
        public static void Initialize(GtdWebContext context)
        {
            context.Database.EnsureCreated();
            if(context.Productos.Any())
            {
                return;
            }

            var productos = new Producto[]
            {
                new Producto{ ID = "1",  NombreProducto = "Camisa" },
                new Producto{ ID = "2",  NombreProducto = "Pantalon" },
                new Producto{ ID = "3",  NombreProducto = "Zapatos" },
                new Producto{ ID = "4",  NombreProducto = "Tenis" },
                new Producto{ ID = "5",  NombreProducto = "Falda" },
                new Producto{ ID = "6",  NombreProducto = "Blusa" }
            };

            foreach (Producto s in productos)
            {
                context.Productos.Add(s);
            }
            context.SaveChanges();
        }
    }
}
