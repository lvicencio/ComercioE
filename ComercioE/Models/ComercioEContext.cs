using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ComercioE.Models
{
    public class ComercioEContext : DbContext
    {
        public ComercioEContext() : base("DefaultConnection")
        {

        }
        //deshabilitacion del borrado en cascada masiva
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        public DbSet<Provincia> Provincias { get; set; }

        public DbSet<Ciudad> Ciudads { get; set; }

        public DbSet<Compania> Companias { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Impuesto> Impuestoes { get; set; }

        public DbSet<Producto> Productoes { get; set; }

        public DbSet<Bodega> Bodegas { get; set; }

        public DbSet<Inventario> Inventarios { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Estado> Estadoes { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetalle> OrderDetalles { get; set; }

        public DbSet<OrderDetalleTmp> OrderDetalleTmps { get; set; }
        public DbSet<CompaniaCliente> CompaniaClientes { get; set; }
    }
}