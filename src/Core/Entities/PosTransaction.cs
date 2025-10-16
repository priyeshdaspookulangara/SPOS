using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class PosTransaction
{
    public int Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal TotalAmount { get; set; }

    public int PaymentMethodId { get; set; }
    [ForeignKey("PaymentMethodId")]
    public PaymentMethod? PaymentMethod { get; set; }

    public ICollection<PosTransactionItem> Items { get; set; } = new List<PosTransactionItem>();
}