using Gestion.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Gestion.Web.Data
{
    public class DataContext : IdentityDbContext<Usuarios>
    {
        public virtual DbSet<Localidades> Localidades { get; set; }
        public virtual DbSet<Provincia> Provincias { get; set; }
        public virtual DbSet<ParamTiposDocumentos> ParamTiposDocumentos { get; set; }
        public virtual DbSet<Categorias> Categorias { get; set; }
        public virtual DbSet<Etiquetas> Etiquetas { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<ProductosCategorias> ProductosCategorias { get; set; }
        public virtual DbSet<ProductosEtiquetas> ProductosEtiquetas { get; set; }
        public virtual DbSet<ProductosImagenes> ProductosImagenes { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderDetailTemp> OrderDetailTemps { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }

}
