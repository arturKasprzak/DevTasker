using DevTasker.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevTasker.Infrastructure;

public class DevTaskerDbContext : DbContext
{
    public DevTaskerDbContext(DbContextOptions<DevTaskerDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
}
