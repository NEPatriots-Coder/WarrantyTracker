using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WarrantyTracker.Models;

namespace WarrantyTracker.Data
{
    public class WarrantyContext : DbContext
    {
        public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Warranty> Warranties { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Model)
                .WithMany(m => m.Products)
                .HasForeignKey(p => p.ModelID);

            modelBuilder.Entity<Warranty>()
                .HasOne(w => w.Product)
                .WithMany(p => p.Warranties)
                .HasForeignKey(w => w.ProductID);

            modelBuilder.Entity<MaintenanceRequest>()
                .HasOne(m => m.Product)
                .WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(m => m.ProductID);
        }
    }
}