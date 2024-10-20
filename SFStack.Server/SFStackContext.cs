using Microsoft.EntityFrameworkCore;
using SFStack.Server.Modules;

namespace SFStack.Server;

public class SFStackContext(DbContextOptions<SFStackContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => new { u.Email })
            .IsUnique();
    }
}