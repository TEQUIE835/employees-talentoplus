namespace _2.Domain.Entities;

public class EducationalLevel
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    
    public List<Employee> Employees { get; set; } = new List<Employee>();
}