using _2.Domain.Entities;

namespace _1.Application.Interfaces.DepartmentInterfaces;

public interface IDepartmentRepository
{
    Task<Department?> GetByIdAsync(int id);
    Task<Department?> GetByNameAsync(string name);
    Task<List<Department>> GetAllAsync();

    Task<Department> AddAsync(Department department);

}