using _2.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3.Infrastructure.Persistence.Configurations;

public class EducationalLevelConfiguration : IEntityTypeConfiguration<EducationalLevel>
{
    public void Configure(EntityTypeBuilder<EducationalLevel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description)
            .IsRequired();
    }
}