using _2.Domain.Entities;

namespace _1.Application.Interfaces.AuthInterfaces;

public interface IJwtService
{
    string GenerateAccessToken(Employee employee);
    string GenerateRefreshToken();
}