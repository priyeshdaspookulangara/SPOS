using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class GlEntry
{
    public int Id { get; set; }

    public int AccountId { get; set; }
    [ForeignKey("AccountId")]
    public ChartOfAccounts? Account { get; set; }

    public DateTime PostingDate { get; set; }
    public decimal Amount { get; set; } // Positive for Debit, Negative for Credit
    public string DocumentType { get; set; } = string.Empty; // e.g., "SA" for G/L, "RV" for Billing
    public string Description { get; set; } = string.Empty;
}