using Microsoft.EntityFrameworkCore;
using Proyecto_Lola.Models;

namespace Proyecto_Lola.Data
{
    public class LolaDbContext : DbContext
    {
        public LolaDbContext(DbContextOptions options):base(options){ }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas{ get; set; }
        public DbSet<Proveedores> Proveedores{ get; set; }
        public DbSet<Stock> Stock{ get; set; }
        public DbSet<Proyecto_Lola.Models.DetalleVenta> DetalleVenta { get; set; } = default!;

        public DbSet<Sucursal> Sucursal { get; set; }

        public DbSet<ProductoSucursal> ProductosSucursal => Set<ProductoSucursal>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sucursal>(entity =>
            {
                entity.HasKey(x => x.SucursalID);

                entity.Property(x => x.Codigo)
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(x => x.Nombre)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.HasIndex(x => x.Codigo)
                      .IsUnique();
            });

            modelBuilder.Entity<ProductoSucursal>(entity =>
            {
                entity.HasKey(x => x.ProductoSucursalId);

                entity.Property(x => x.precioVenta)
                      .HasColumnType("decimal(18,2)");


                entity.HasIndex(x => new { x.SucursalId, x.ProductoId })
                      .IsUnique();

                entity.HasOne(x => x.sucursal)
                      .WithMany(s => s.ProductosSucursal)
                      .HasForeignKey(x => x.SucursalId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.producto)
                      .WithMany() // luego podés agregar navegación en Producto si querés
                      .HasForeignKey(x => x.ProductoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });


        }

    }
}

