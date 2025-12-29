using _1.Application.Interfaces.AuthInterfaces;
using _2.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken token)
    {
        _context.Add(token);
        await  _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .FindAsync(token);
    }

    public async Task<List<RefreshToken>> GetByEmployeeAsync(int employeeId)
    {
        return await _context.RefreshTokens.Where(e => e.EmployeeId == employeeId).ToListAsync();
    }

    public async Task RevokeTokenAsync(RefreshToken token)
    {
        _context.RefreshTokens.Remove(token);
        await _context.SaveChangesAsync();
    }

    public Task RevokeAllForEmployeeAsync(int employeeId)
    {
        var tokens = _context.RefreshTokens.Where(t => t.EmployeeId == employeeId);
        _context.RefreshTokens.RemoveRange(tokens);
        return _context.SaveChangesAsync();
    }
}