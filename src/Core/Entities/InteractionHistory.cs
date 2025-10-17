using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class InteractionHistory
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

    public DateTime InteractionDate { get; set; }
    public string InteractionType { get; set; } = string.Empty; // e.g., "Sales Order", "POS Transaction", "Phone Call"
    public string Channel { get; set; } = string.Empty; // e.g., "Web", "In-Store", "Phone"
    public string Notes { get; set; } = string.Empty;

    // Optional links to other documents
    public int? SalesOrderId { get; set; }
    public int? PosTransactionId { get; set; }
}