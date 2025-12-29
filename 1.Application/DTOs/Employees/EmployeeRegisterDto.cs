using _2.Domain.ValueObjects;

namespace _1.Application.DTOs.Employees;

public class EmployeeRegisterDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Password { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime EntryDate { get; set; }
    public int EducationalLevelId { get; set; }
    public string ProfessionalProfile { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public decimal Salary { get; set; }
    public string Position { get; set; } = string.Empty;
}