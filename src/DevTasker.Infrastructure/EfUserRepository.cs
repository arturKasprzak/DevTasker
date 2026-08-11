using DevTasker.Application;
using DevTasker.Domain;

namespace DevTasker.Infrastructure;

public class EfUserRepository : IUserRepository
{
    private readonly DevTaskerDbContext _devTaskerDbContext;

    public EfUserRepository(DevTaskerDbContext devTaskerDbContext)
    {
        _devTaskerDbContext = devTaskerDbContext;
    }
    public Task<User?> GetByIdAsync(Guid id)
    {
        return _devTaskerDbContext.Users.FindAsync(id).AsTask();
    }
}