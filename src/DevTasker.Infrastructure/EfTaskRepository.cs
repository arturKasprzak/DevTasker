using DevTasker.Application;
using DevTasker.Domain;

using Microsoft.EntityFrameworkCore;

namespace DevTasker.Infrastructure;
public class EfTaskRepository : ITaskRepository
{
    private readonly DevTaskerDbContext _devTaskerDbContext;

    public EfTaskRepository(DevTaskerDbContext devTaskerDbContext)
    {
        _devTaskerDbContext = devTaskerDbContext;
    }
    public async Task<IReadOnlyList<TaskItem>> GetAllTaskItemsAsync()
    {
        return await _devTaskerDbContext.Tasks.Include(x => x.AssignedUser).ToListAsync();
    }
}
