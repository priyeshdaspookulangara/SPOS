namespace Core.Entities;

public class ChartOfAccounts
{
    public int Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty; // e.g., Asset, Liability, Equity, Revenue, Expense
}