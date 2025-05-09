using Microsoft.EntityFrameworkCore;
using Tomou.Domain.Entities;

namespace Tomou.Infrastructure.DataAccess;
internal class TomouDbContext : DbContext
{
    public TomouDbContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Dependent> Dependents => Set<Dependent>();
    public DbSet<Medication> Medications => Set<Medication>();
    public DbSet<MedicationLog> MedicationLogs => Set<MedicationLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasMany(u => u.Dependents)
                .WithOne(d => d.Caregiver)
                .HasForeignKey(d => d.CaregiverId);
        });

        modelBuilder.Entity<Dependent>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.HasMany(d => d.Medications)
                    .WithOne(m => m.Dependent)
                    .HasForeignKey(m => m.DependentId);
        });

        modelBuilder.Entity<Medication>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.HasMany(m => m.Logs)
                    .WithOne(l => l.Medication)
                    .HasForeignKey(l => l.MedicationId);
        });

        modelBuilder.Entity<MedicationLog>(entity =>
        {
            entity.HasKey(l => l.Id);
        });
    }

}
