using Microsoft.EntityFrameworkCore;
using Profiles.Domain.Models;
using System.Reflection;

public class ProfilesDbContext : DbContext
{
    public ProfilesDbContext(DbContextOptions<ProfilesDbContext> options) : base(options) { }

    public DbSet<PatientProfile> Profiles => Set<PatientProfile>();
    public DbSet<Receptionist> Receptionists => Set<Receptionist>();
    public DbSet<DoctorProfile> Doctors => Set<DoctorProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}