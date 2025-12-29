using _2.Domain.Entities;

namespace _1.Application.Interfaces.EmployeeInterfaces;

public interface IEmployeeDepartmentRepository
{
    Task AddAsync(EmployeeDepartment rel);
    Task<List<EmployeeDepartment>> GetByEmployeeIdAsync(int employeeId);
    Task UpdateAsync(EmployeeDepartment department);
}