using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class LeaveRequest
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    [ForeignKey("EmployeeId")]
    public Employee? Employee { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string LeaveType { get; set; } = "Annual"; // e.g., Annual, Sick, Unpaid
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    public string? Reason { get; set; }
    public DateTime DateRequested { get; set; }
}