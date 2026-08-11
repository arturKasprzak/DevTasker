using DevTasker.Domain;

using Microsoft.EntityFrameworkCore;

namespace DevTasker.Infrastructure;

public class DevTaskerDbContext : DbContext
{
    public DevTaskerDbContext(DbContextOptions<DevTaskerDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(x => x.Email).HasMaxLength(256);
            entity.Property(x => x.Name).HasMaxLength(256);

            entity.HasData(new
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Email = "test.user@devtasker.com",
                Name = "Test User"
            });
        });
    }
}