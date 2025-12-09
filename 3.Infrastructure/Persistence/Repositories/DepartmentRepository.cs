using _1.Application.Interfaces.DepartmentInterfaces;
using _2.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3.Infrastructure.Persistence.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;
    public DepartmentRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _context.Departments.FindAsync(id);
    }

    public async Task<Department?> GetByNameAsync(string name)
    {
        return await _context.Departments
            .FirstOrDefaultAsync(d => d.Name == name);
    }

    public async Task<List<Department>> GetAllAsync()
    {
        return await _context.Departments.ToListAsync(); 
    }

    public async Task<Department> AddAsync(Department department)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return department;
    }
}