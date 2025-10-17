using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Opportunity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

    public int? ContactId { get; set; }
    [ForeignKey("ContactId")]
    public Contact? Contact { get; set; }

    public decimal EstimatedValue { get; set; }
    public DateTime CloseDate { get; set; }
    public string Stage { get; set; } = "Prospecting"; // Prospecting, Proposal, Negotiation, Closed-Won, Closed-Lost
}