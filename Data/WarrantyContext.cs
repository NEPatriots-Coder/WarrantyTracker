using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WarrantyTracker.Models;

namespace WarrantyTracker.Data
{
    public class WarrantyContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public WarrantyContext()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<WarrantyContext>();  // Add this line

            _configuration = builder.Build();
        }

        public DbSet<PurchaseItem> PurchaseItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                }

                var serverVersion = ServerVersion.AutoDetect(connectionString);
                
                optionsBuilder
                    .UseMySql(connectionString, serverVersion)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PurchaseItem>(entity =>
            {
                entity.ToTable("PRODUCT");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ProductID").ValueGeneratedOnAdd();
                entity.Property(e => e.ModelID).IsRequired();
                entity.Property(e => e.ItemName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SerialNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.WarrantyPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MaintenanceNotes).HasColumnType("text");
            });
        }
    }
}