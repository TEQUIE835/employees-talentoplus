using _2.Domain.Entities;

namespace _1.Application.Interfaces.EducationalLevelInterfaces;

public interface IEducationalLevelRepository
{
    Task<EducationalLevel?> GetByIdAsync(int id);
    Task<EducationalLevel?> GetByDescriptionAsync(string description);
    Task<List<EducationalLevel>> GetAllAsync();

    Task<EducationalLevel> AddAsync(EducationalLevel level);
}