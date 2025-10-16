using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class BillingDocument
{
    public int Id { get; set; }

    public int DeliveryId { get; set; }
    [ForeignKey("DeliveryId")]
    public Delivery? Delivery { get; set; }

    public DateTime BillingDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Open"; // Open, Cleared
}