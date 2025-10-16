using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class PurchaseRequisition
{
    public int Id { get; set; }
    public int MaterialId { get; set; }
    [ForeignKey("MaterialId")]
    public Material? Material { get; set; }
    public int Quantity { get; set; }
    public DateTime DateRequested { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Ordered
}