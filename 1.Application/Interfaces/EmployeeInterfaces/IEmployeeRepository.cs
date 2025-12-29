using _2.Domain.Entities;

namespace _1.Application.Interfaces.EmployeeInterfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee?> GetByEmailAsync(string email);
    Task<Employee?> GetByDocumentAsync(string document);
    Task<List<Employee>> GetAllAsync();
    

    Task<Employee?> AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Employee employee);
    
    
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByDocumentAsync(string document);
}