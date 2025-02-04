using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WarrantyTracker.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public int ModelID { get; set; }

        [Required]
        [StringLength(50)]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        [StringLength(100)]
        public string? RetailerName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("ModelID")]
        public virtual ProductModel Model { get; set; } = null!;

        public virtual ICollection<Warranty> Warranties { get; set; } = new List<Warranty>();

        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
    }
}