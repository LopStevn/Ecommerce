using Ecommerce.Models.EN;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Models.DATA
{
    public class EcommerceDbContext : IdentityDbContext<Usuario>
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {

        }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Producto> Productos { get;set; }

        public DbSet<VentaTemporal> VentasTemporales { get; set; }

        public DbSet<DetalleVenta> DetallesVenta { get; set; }

        public DbSet<Venta> Ventas { get; set; }
    }
}
