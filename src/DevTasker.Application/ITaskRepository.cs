using DevTasker.Domain;

namespace DevTasker.Application;
public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItem>> GetAllTaskItemsAsync();
}
