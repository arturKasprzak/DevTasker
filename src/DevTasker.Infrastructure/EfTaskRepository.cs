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

    public async Task<TaskItem?> RemoveTaskItemAsync(Guid id)
    {
        var taskItem = await _devTaskerDbContext.Tasks.FirstOrDefaultAsync(x => x.Id == id);
        if (taskItem != null)
        {
            _devTaskerDbContext.Tasks.Remove(taskItem);
            await _devTaskerDbContext.SaveChangesAsync();
        }
        return taskItem;
    }

    public async Task<TaskItem?> UpdateTaskItemAsync(Guid id, UpdateTaskItemRequest request)
    {
        var taskItem = await _devTaskerDbContext.Tasks.FirstOrDefaultAsync(x => x.Id == id);
        if (taskItem != null)
        {
            taskItem.ChangeTitle(request.Title);
            taskItem.ChangeDescription(request.Description);

            if (request.Status == true)
            {
                taskItem.MarkAsCompleted();
            }
            else
            {
                taskItem.MarkAsIncomplete();
            }

            if(request.UserId.HasValue)
            {
                var user = await _devTaskerDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
                if(user is null)
                {
                    throw new UserNotFoundException(request.UserId.Value);
                }
                taskItem.AssignTo(user);
            }
            else
            {
                taskItem.AssignTo(null);
            }
                
            await _devTaskerDbContext.SaveChangesAsync();
        }

        return taskItem;
    }
}
