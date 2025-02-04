using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarrantyTracker.Models
{
    public class MaintenanceRequest
    {
        [Key]
        public int RequestID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Pending";  // Pending, InProgress, Completed, Cancelled

        public string? Resolution { get; set; }

        public DateTime? CompletionDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Cost { get; set; }

        public string? TechnicianNotes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; } = null!;
    }
}