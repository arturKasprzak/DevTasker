using DevTasker.Domain;

using Microsoft.EntityFrameworkCore;

namespace DevTasker.Infrastructure;

public class DevTaskerDbContext : DbContext
{
    public DevTaskerDbContext(DbContextOptions<DevTaskerDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }

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

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.Property(x => x.Title).HasMaxLength(256);
            entity.HasOne(t => t.AssignedUser)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasData(new
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Title = "Add TextBox",
                Status = true,
                AssignedUserId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            });
        });
    }
}