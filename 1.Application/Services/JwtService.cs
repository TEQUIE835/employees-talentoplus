using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using _1.Application.Interfaces.AuthInterfaces;
using _2.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace _1.Application.Services;

public class JwtService : IJwtService
{
    private readonly string _jwtKey;
    private readonly int _TokenExpirationInMinutes;
    private readonly string _issuer;
    private readonly string _audience;
    public JwtService(string jwtKey, int tokenExpirationInMinutes, string issuer, string audience)
    {
        _jwtKey = jwtKey;
        _TokenExpirationInMinutes = tokenExpirationInMinutes;
        _issuer = issuer;
        _audience = audience;
    }
    public string GenerateAccessToken(Employee employee)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtKey);
        var claims = new []
        {
            new Claim(JwtRegisteredClaimNames.Sub, employee.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, employee.Email.Value),
            new Claim("Document", employee.Document)
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_TokenExpirationInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        return Convert.ToBase64String(randomBytes);
    }
}