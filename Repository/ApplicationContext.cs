using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Repository;

public class ApplicationContext : IdentityDbContext<User>
{
    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        //modelBuilder.ApplyConfiguration(new ShiftConfiguration());
        //modelBuilder.ApplyConfiguration(new WorkerConfiguration());
    }

    public DbSet<Worker> Workers => Set<Worker>();
    public DbSet<Shift> Shifts => Set<Shift>();
}
