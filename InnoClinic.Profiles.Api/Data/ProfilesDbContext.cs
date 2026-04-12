using InnoClinic.Profiles.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

public class ProfilesDbContext : DbContext
{
    public ProfilesDbContext(DbContextOptions<ProfilesDbContext> options) : base(options) { }

    public DbSet<PatientProfile> Profiles => Set<PatientProfile>();
    public DbSet<Receptionist> Receptionists => Set<Receptionist>();
    public DbSet<DoctorProfile> Doctors => Set<DoctorProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PatientProfile>(entity =>
        {
            entity.ToTable("PatientProfiles");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .IsRequired(false);

            entity.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.DateOfBirth)
                .IsRequired()
                .HasColumnType("date");

            entity.Property(e => e.IsEmailVerified)
                .HasDefaultValue(false);

            entity.Property(e => e.IsLinkedToAccount)
                .HasDefaultValue(false);

            entity.HasIndex(e => new { e.LastName, e.FirstName, e.MiddleName });
        });

        modelBuilder.Entity<DoctorProfile>(entity =>
        {
            entity.ToTable("Doctors");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.MiddleName)
                .HasMaxLength(100);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Specialization)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.OfficeAddress)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
        });

        }
}