using System.Text;
using _1.Application.Interfaces.AuthInterfaces;
using _1.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using IEmailSender = _1.Application.Interfaces.EmailInterfaces.IEmailSender;

namespace _1.Application;

public static class DependencyInjection 
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey =
            configuration["Jwt:Key"] ??
            configuration["SecretKey"] ??
            Environment.GetEnvironmentVariable("SECRET_KEY");

        var jwtIssuer =
            configuration["Jwt:Issuer"] ??
            configuration["Issuer"] ??
            Environment.GetEnvironmentVariable("ISSUER");

        var jwtAudience =
            configuration["Jwt:Audience"] ??
            configuration["Audience"] ??
            Environment.GetEnvironmentVariable("AUDIENCE");

        var expirationInMinutes = int.Parse(configuration["Jwt:ExpirationInMinutes"] ?? "7");
        if (string.IsNullOrWhiteSpace(jwtKey))
            throw new InvalidOperationException("JWT Key is not configured. Set Jwt:Key or SECRET_KEY.");

        services.AddSingleton<IJwtService>(provider =>
        {
            return new JwtService(jwtKey!, expirationInMinutes, jwtIssuer, jwtAudience);
        });
       
       
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = !string.IsNullOrWhiteSpace(jwtIssuer),
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = !string.IsNullOrWhiteSpace(jwtAudience),
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });
        
        services.AddScoped<IPasswordHasher,PasswordHasherService>();
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<EmployeeServices>();
        services.AddScoped<DepartmentService>();
        services.AddScoped<EducationalLevelService>();
        return services;
    }
}