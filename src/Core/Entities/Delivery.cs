using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Delivery
{
    public int Id { get; set; }

    public int SalesOrderId { get; set; }
    [ForeignKey("SalesOrderId")]
    public SalesOrder? SalesOrder { get; set; }

    public DateTime DeliveryDate { get; set; }
    public string Status { get; set; } = "Picking"; // Picking, Shipped, Delivered
}