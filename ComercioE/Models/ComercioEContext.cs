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

        public System.Data.Entity.DbSet<ComercioE.Models.Ciudad> Ciudads { get; set; }

        public System.Data.Entity.DbSet<ComercioE.Models.Compania> Companias { get; set; }

        public System.Data.Entity.DbSet<ComercioE.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<ComercioE.Models.Categoria> Categorias { get; set; }

        public System.Data.Entity.DbSet<ComercioE.Models.Impuesto> Impuestoes { get; set; }

        public System.Data.Entity.DbSet<ComercioE.Models.Producto> Productoes { get; set; }

        public System.Data.Entity.DbSet<ComercioE.Models.Bodega> Bodegas { get; set; }
    }
}