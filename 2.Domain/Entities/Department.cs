namespace _2.Domain.Entities;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public List<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();
}