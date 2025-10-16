using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class StockLedger
{
    public int Id { get; set; }
    public int MaterialId { get; set; }
    [ForeignKey("MaterialId")]
    public Material? Material { get; set; }

    public int QuantityChange { get; set; } // Positive for goods receipt, negative for goods issue
    public int NewQuantity { get; set; }
    public string MovementType { get; set; } = string.Empty; // e.g., "Goods Receipt PO", "Goods Issue SO"
    public DateTime MovementDate { get; set; }
    public int? PurchaseOrderId { get; set; }
    [ForeignKey("PurchaseOrderId")]
    public PurchaseOrder? PurchaseOrder { get; set; }
}