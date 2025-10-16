namespace Core.Entities;

public class PaymentMethod
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // e.g., "Cash", "Credit Card"

    // This could be linked to a specific G/L account for cash/bank postings
    public int GlAccountId { get; set; }
}