using GtdWebAspNetCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GtdWebAspNetCore.Data
{
    public class GtdWebContext : DbContext
    {
        public GtdWebContext(DbContextOptions<GtdWebContext> options) : base(options)
        {

        }

        public virtual DbSet<Producto> Productos { get; set; }
        
        public virtual DbSet<Factura> Facturas { get; set; }

        public virtual DbSet<Detalle> Detalles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>().ToTable("Producto");
            modelBuilder.Entity<Factura>().ToTable("Factura");
            modelBuilder.Entity<Detalle>().ToTable("Detalle");
        }

    }
}
