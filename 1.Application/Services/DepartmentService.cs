
using _1.Application.Interfaces.DepartmentInterfaces;
using _2.Domain.Entities;

namespace _1.Application.Services;

public class DepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return departments;
    }
    
    public async Task<Department?> GetByIdAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
            throw new Exception("Department not found");
        
        return department;
    }
    
    public async Task<Department> GetByNameAsync(string name)
    {
        var department = await _departmentRepository.GetByNameAsync(name);
        if (department == null)
            throw new Exception("Department not found");
        return department;
    }
    
    public async Task CreateAsync(Department department)
    {
        await _departmentRepository.AddAsync(department);
    }
}