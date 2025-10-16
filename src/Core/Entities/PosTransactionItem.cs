using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class PosTransactionItem
{
    public int Id { get; set; }

    public int PosTransactionId { get; set; }
    [ForeignKey("PosTransactionId")]
    public PosTransaction? PosTransaction { get; set; }

    public int MaterialId { get; set; }
    [ForeignKey("MaterialId")]
    public Material? Material { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}