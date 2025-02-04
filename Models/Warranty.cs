using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarrantyTracker.Models
{
    public class Warranty
    {
        [Key]
        public int WarrantyID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        [StringLength(50)]
        public string WarrantyType { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string? Terms { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

        // Navigation property to link back to Product
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; } = null!;
    }
}