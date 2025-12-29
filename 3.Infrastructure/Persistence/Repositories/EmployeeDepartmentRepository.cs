using _1.Application.Interfaces.EmployeeInterfaces;
using _2.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3.Infrastructure.Persistence.Repositories;

public class EmployeeDepartmentRepository : IEmployeeDepartmentRepository
{
    private readonly AppDbContext _context;
    public EmployeeDepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(EmployeeDepartment rel)
    {
        _context.EmployeeDepartments.Add(rel);
        await  _context.SaveChangesAsync();
    }

    public async Task<List<EmployeeDepartment>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _context.EmployeeDepartments.Where(ed => ed.EmployeeId == employeeId).Include(ed => ed.Employee).Include(ed => ed.Department).ToListAsync();
    }

    public async Task UpdateAsync(EmployeeDepartment department)
    {
        _context.EmployeeDepartments.Update(department);
        await _context.SaveChangesAsync();
    }
}