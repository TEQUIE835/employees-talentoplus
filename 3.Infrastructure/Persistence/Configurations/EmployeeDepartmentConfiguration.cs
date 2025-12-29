using _2.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3.Infrastructure.Persistence.Configurations;

public class EmployeeDepartmentConfiguration : IEntityTypeConfiguration<EmployeeDepartment>
{
    public void Configure(EntityTypeBuilder<EmployeeDepartment> builder)
    {
        builder.HasKey(x => new { x.EmployeeId, x.DepartmentId });
        builder.HasOne(ed => ed.Employee)
            .WithMany(e => e.EmployeeDepartments)
            .HasForeignKey(ed => ed.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        builder.HasOne(ed => ed.Department).
            WithMany(d => d.EmployeeDepartments)
            .HasForeignKey(ed => ed.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        builder.Property(x => x.Position)
            .IsRequired();
        builder.Property(x => x.Salary).IsRequired();
        builder.Property(ed => ed.EmployeeStatus)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

    }
}