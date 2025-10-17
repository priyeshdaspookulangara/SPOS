namespace Core.Entities;

public class Lead
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Status { get; set; } = "New"; // New, Contacted, Qualified, Disqualified
    public string Source { get; set; } = "Web"; // Web, Referral, etc.
}