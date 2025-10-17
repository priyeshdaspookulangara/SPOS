using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Address
{
    public int Id { get; set; }
    public string AddressType { get; set; } = "Billing"; // e.g., "Billing", "Shipping"
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public int CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }
}