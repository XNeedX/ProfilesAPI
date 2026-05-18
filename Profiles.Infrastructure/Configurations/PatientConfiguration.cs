using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profiles.Domain.Models;

namespace Profiles.Infrastructure.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<PatientProfile>
{
    public void Configure(EntityTypeBuilder<PatientProfile> builder)
    {
        builder.ToTable("PatientProfiles");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.MiddleName)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(e => e.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.DateOfBirth)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(e => e.IsEmailVerified)
            .HasDefaultValue(false);

        builder.Property(e => e.IsLinkedToAccount)
            .HasDefaultValue(false);

        builder.HasIndex(e => new { e.LastName, e.FirstName, e.MiddleName });
    }
}