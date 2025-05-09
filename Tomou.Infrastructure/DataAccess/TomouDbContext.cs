using Microsoft.EntityFrameworkCore;
using Tomou.Domain.Entities;

namespace Tomou.Infrastructure.DataAccess;
internal class TomouDbContext : DbContext
{
    public TomouDbContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Dependent> Dependents => Set<Dependent>();
    public DbSet<Medication> Medicication => Set<Medication>();
    public DbSet<MedicationLog> MedicationLogs => Set<MedicationLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}
