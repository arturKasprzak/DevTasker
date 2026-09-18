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

    public async Task AddNewTaskItemAsync(TaskItem item)
    {
        _devTaskerDbContext.Tasks.Add(item);
        await _devTaskerDbContext.SaveChangesAsync();
    }

    public async Task<TaskItem?> GetTaskItemAsync(Guid id)
    {
        return await _devTaskerDbContext.Tasks.Include(x => x.AssignedUser).FirstOrDefaultAsync(x => x.Id == id);
    }
}
