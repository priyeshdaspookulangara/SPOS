using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class SalesOrderItem
{
    public int Id { get; set; }

    public int SalesOrderId { get; set; }
    [ForeignKey("SalesOrderId")]
    public SalesOrder? SalesOrder { get; set; }

    public int MaterialId { get; set; }
    [ForeignKey("MaterialId")]
    public Material? Material { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}