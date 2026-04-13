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
                modelBuilder.Entity<Receptionist>(entity =>
        {
            entity.ToTable("Receptionists");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.OfficeAddress).IsRequired().HasMaxLength(250);
        });
    }
}