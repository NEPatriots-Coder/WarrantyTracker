

namespace WarrantyTracker.Models;

public class PurchaseItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public DateTime WarrantyExpirationDate { get; set; }
    public decimal PurchasePrice { get; set; }
    public string MaintenanceNotes { get; set; } = string.Empty;
}