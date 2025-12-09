namespace _2.Domain.Entities;

public class EmployeeDepartment
{
    public int EmployeeId { get; set; }
    public int DepartmentId { get; set; }
    public string Position { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public Status EmployeeStatus { get; set; }
    
    public Employee Employee { get; set; } = new Employee();
    public Department Department { get; set; } = new Department();
}
public enum Status
{
    Active,
    Inactive,
    OnVacation,
    Suspended
}