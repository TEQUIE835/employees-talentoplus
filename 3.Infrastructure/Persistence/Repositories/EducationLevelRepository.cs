using _1.Application.Interfaces.EducationalLevelInterfaces;
using _2.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3.Infrastructure.Persistence.Repositories;

public class EducationLevelRepository : IEducationalLevelRepository
{
    private readonly AppDbContext _context;
    public EducationLevelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EducationalLevel?> GetByIdAsync(int id)
    {
        return await _context.EducationalLevels.FindAsync(id);
    }

    public async Task<EducationalLevel?> GetByDescriptionAsync(string description)
    {
        return await _context.EducationalLevels.FirstOrDefaultAsync(el => el.Description == description); 
    }

    public async Task<List<EducationalLevel>> GetAllAsync()
    {
        return await _context.EducationalLevels.ToListAsync();
    }

    public async Task<EducationalLevel> AddAsync(EducationalLevel level)
    {
        _context.EducationalLevels.Add(level);
        await _context.SaveChangesAsync();
        return level;
    }
}