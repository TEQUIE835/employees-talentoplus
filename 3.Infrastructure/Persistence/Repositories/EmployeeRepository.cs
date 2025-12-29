using _1.Application.Interfaces.EmployeeInterfaces;
using _2.Domain.Entities;
using _2.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace _3.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;
    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees.Include(e => e.EducationalLevel).FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Employee?> GetByEmailAsync(string email)
    {
        return await _context.Employees.FirstOrDefaultAsync(e => e.Email.Value == email);
    }

    public async Task<Employee?> GetByDocumentAsync(string document)
    {
        return await _context.Employees.FirstOrDefaultAsync(e => e.Document == document);
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _context.Employees.ToListAsync();
    }

    public async Task<Employee?> AddAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync(); 
    }

    public async Task DeleteAsync(Employee employee)
    {
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync(); 
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Employees.AnyAsync(e => e.Email.Value.ToString() == email);
    }

    public async Task<bool> ExistsByDocumentAsync(string document)
    {
        return await _context.Employees.AnyAsync(e => e.Document == document);
    }
}