using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class PurchaseOrder
{
    public int Id { get; set; }
    public int VendorId { get; set; }
    [ForeignKey("VendorId")]
    public Vendor? Vendor { get; set; }

    public int MaterialId { get; set; }
    [ForeignKey("MaterialId")]
    public Material? Material { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;

    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = "Open"; // Open, Closed
}