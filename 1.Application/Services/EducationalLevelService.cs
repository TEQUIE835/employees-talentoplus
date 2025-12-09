using _1.Application.Interfaces.EducationalLevelInterfaces;
using _2.Domain.Entities;

namespace _1.Application.Services;

public class EducationalLevelService
{
    private readonly IEducationalLevelRepository _educationalLevelRepository;
    public EducationalLevelService(IEducationalLevelRepository educationalLevelRepository)
    {
        _educationalLevelRepository = educationalLevelRepository;
    }

    public async Task<IEnumerable<EducationalLevel>> GetAllAsync()
    {
        var educationalLevels = await _educationalLevelRepository.GetAllAsync();
        return educationalLevels;
    }
    public async Task<EducationalLevel?> GetByIdAsync(int id)
    {
        var educationalLevel = await _educationalLevelRepository.GetByIdAsync(id);
        if (educationalLevel == null) 
            throw new Exception("Educational level not found");
        return educationalLevel;
    }
    public async Task CreateAsync(EducationalLevel educationalLevel)
    {
        await _educationalLevelRepository.AddAsync(educationalLevel);
    }
    public async Task<EducationalLevel?> GetByDescriptionAsync(string description)
    {
        var educationalLevel = await _educationalLevelRepository.GetByDescriptionAsync(description);
        if (educationalLevel == null)
            throw new Exception("Educational level not found");
        return educationalLevel;
    }
}
    