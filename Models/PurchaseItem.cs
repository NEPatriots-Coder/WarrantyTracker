using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarrantyTracker.Models
{
    [Table("PRODUCT")]
    public class PurchaseItem
    {
        [Key]
        [Column("ProductID")]
        public int Id { get; set; }

        [Required]
        public int ModelID { get; set; }

        [Required]
        [StringLength(100)]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public DateTime WarrantyExpiration { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal WarrantyPrice { get; set; }

        public string MaintenanceNotes { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
    }
}