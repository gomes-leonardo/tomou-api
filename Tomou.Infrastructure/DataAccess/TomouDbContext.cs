using Microsoft.EntityFrameworkCore;
using Tomou.Domain.Entities;

namespace Tomou.Infrastructure.DataAccess;
public class TomouDbContext : DbContext
{
    public TomouDbContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Dependent> Dependents => Set<Dependent>();
    public DbSet<Medication> Medications => Set<Medication>();
    public DbSet<MedicationLog> MedicationLogs => Set<MedicationLog>();

    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasMany(u => u.Dependents)
                .WithOne(d => d.Caregiver)
                .HasForeignKey(d => d.CaregiverId);
            entity.HasMany(u => u.PasswordResetTokens)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);
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

            entity.HasOne(m => m.Dependent)
                  .WithMany(d => d.Medications)
                  .HasForeignKey(m => m.DependentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.User)
                  .WithMany(u => u.Medications)
                  .HasForeignKey(m => m.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(m => m.TimesToTake)
                .HasConversion(
                    v => string.Join(',', v.Select(t => t.ToString("HH:mm"))),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(TimeOnly.Parse).ToList()
            );

            entity.HasMany(m => m.Logs)
                  .WithOne(l => l.Medication)
                  .HasForeignKey(l => l.MedicationId);

        });


        modelBuilder.Entity<MedicationLog>(entity =>
        {
            entity.HasKey(l => l.Id);

            entity.Property(l => l.ScheduledFor)
                  .IsRequired();

            entity.Property(l => l.Status)
                  .HasConversion<string>()
                  .IsRequired();

            entity.HasOne(l => l.Medication)
                  .WithMany(m => m.Logs)
                  .HasForeignKey(l => l.MedicationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Token)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(p => p.ExpiresAt)
                  .IsRequired();

            entity.Property(p => p.Used)
                  .HasDefaultValue(false);

            entity.HasOne(p => p.User)
                  .WithMany(u => u.PasswordResetTokens) 
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

}
