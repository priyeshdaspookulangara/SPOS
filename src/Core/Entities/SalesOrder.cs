using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class SalesOrder
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = "Open"; // Open, Delivered, Invoiced, Closed
    public decimal TotalAmount { get; set; }

    public ICollection<SalesOrderItem> Items { get; set; } = new List<SalesOrderItem>();
}