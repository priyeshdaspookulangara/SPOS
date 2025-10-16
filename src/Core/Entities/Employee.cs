using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime DateOfJoining { get; set; }

    public int DepartmentId { get; set; }
    [ForeignKey("DepartmentId")]
    public Department? Department { get; set; }

    public int PayGradeId { get; set; }
    [ForeignKey("PayGradeId")]
    public PayGrade? PayGrade { get; set; }
}