using System.Reflection;
using _2.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EducationalLevel> EducationalLevels { get; set; }
    public DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}