using AspTestStage.Database.Domain;
using Microsoft.EntityFrameworkCore;

namespace AspTestStage.Database;

public class AppDbContext : DbContext
{
    public AppDbContext (DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupStudent> GroupStudents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var roles = modelBuilder.Entity<Role>();
        roles.HasMany(r => r.Users).WithOne(r => r.Role);
    }
}