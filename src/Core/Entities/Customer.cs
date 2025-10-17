namespace Core.Entities;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CustomerType { get; set; } = "Individual"; // "Individual" or "Corporate"
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    // Navigation Properties
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    public ICollection<InteractionHistory> Interactions { get; set; } = new List<InteractionHistory>();
    public ICollection<Relationship> ParentRelationships { get; set; } = new List<Relationship>();
    public ICollection<Relationship> ChildRelationships { get; set; } = new List<Relationship>();
}