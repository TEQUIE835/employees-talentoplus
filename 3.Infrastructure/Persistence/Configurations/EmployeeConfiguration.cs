using _2.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.FirstName)
            .IsRequired();

        builder.Property(x => x.LastName)
            .IsRequired();

        builder.OwnsOne(x => x.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired();
            email.HasIndex(e => e.Value).IsUnique();
        });

        builder.Property(x => x.DateOfBirth)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.Property(x => x.Address)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .IsRequired();

        builder.Property(x => x.EntryDate)
            .IsRequired();

        builder.Property(x => x.ProfessionalProfile)
            .IsRequired();

        builder.HasOne(x => x.EducationalLevel)
            .WithMany(el => el.Employees)
            .HasForeignKey(x => x.EducationalLevelId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        builder.HasIndex(x => x.Document).IsUnique();
    }
}