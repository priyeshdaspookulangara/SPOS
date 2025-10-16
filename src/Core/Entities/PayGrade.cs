namespace Core.Entities;

public class PayGrade
{
    public int Id { get; set; }
    public string Grade { get; set; } = string.Empty;
    public decimal MonthlySalary { get; set; }
}