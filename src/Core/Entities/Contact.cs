using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Contact
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Role { get; set; } // e.g., "Primary Contact", "Billing Contact"

    public int CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }
}