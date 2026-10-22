
using DevTasker.Domain;

namespace DevTasker.Application;
public interface ITaskService
{
    Task AddNewTaskItemAsync(TaskItem item);
    Task<IReadOnlyList<TaskItem>> GetAllTaskItemsAsync();
    Task<TaskItem?> GetTaskItemAsync(Guid id);
    Task<TaskItem?> RemoveTaskItemAsync(Guid id);
    Task<TaskItem?> UpdateTaskItemAsync(Guid id, UpdateTaskItemRequest request);
}