using _2.Domain.Entities;

namespace _1.Application.Interfaces.AuthInterfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<List<RefreshToken>> GetByEmployeeAsync(int employeeId);

    Task RevokeTokenAsync(RefreshToken token);
    Task RevokeAllForEmployeeAsync(int employeeId);
}