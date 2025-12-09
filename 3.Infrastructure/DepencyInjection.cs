using System.Text;
using _1.Application.Interfaces.AuthInterfaces;
using _1.Application.Interfaces.DepartmentInterfaces;
using _1.Application.Interfaces.EducationalLevelInterfaces;
using _1.Application.Interfaces.EmployeeInterfaces;
using _3.Infrastructure.Persistence;
using _3.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace _3.Infrastructure;

public static class DepencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(ServerVersion.AutoDetect(connectionString))));
        services.AddScoped<IDepartmentRepository,DepartmentRepository>();
        services.AddScoped<IEducationalLevelRepository, EducationLevelRepository>();
        services.AddScoped<IEmployeeDepartmentRepository,EmployeeDepartmentRepository>();
        services.AddScoped<IEmployeeRepository,EmployeeRepository>();
        services.AddScoped<IRefreshTokenRepository,RefreshTokenRepository>();
        return services;
    }
}