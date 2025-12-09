using _2.Domain.ValueObjects;

namespace _2.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public Email Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime EntryDate { get; set; }
    public int EducationalLevelId { get; set; }
    public string ProfessionalProfile { get; set; } = string.Empty;
    
    public EducationalLevel? EducationalLevel { get; set; }
    
    public List<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();
    
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}