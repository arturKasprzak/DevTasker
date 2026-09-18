using DevTasker.Domain;

namespace DevTasker.Application;
public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItem>> GetAllTaskItemsAsync();
    Task<TaskItem?> GetTaskItemAsync(Guid id);
    Task AddNewTaskItemAsync(TaskItem item);
}
