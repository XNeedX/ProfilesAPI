using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profiles.Domain.Models;

namespace Profiles.Infrastructure.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<DoctorProfile>
{
    public void Configure(EntityTypeBuilder<DoctorProfile> builder)
    {
        builder.ToTable("Doctors");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.PhotoPath)
               .HasColumnName("PhotoPathPath");

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.MiddleName)
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Specialization)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.OfficeAddress)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(e => e.Status)
            .IsRequired()
            .HasMaxLength(50);
    }
}