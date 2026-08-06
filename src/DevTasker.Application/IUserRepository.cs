using DevTasker.Domain;

namespace DevTasker.Application;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
}
