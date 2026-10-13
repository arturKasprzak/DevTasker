
using DevTasker.Domain;

namespace DevTasker.Application;
public class TaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public Task AddNewTaskItemAsync(TaskItem item)
    {
        return _taskRepository.AddNewTaskItemAsync(item);
    }

    public Task<IReadOnlyList<TaskItem>> GetAllTaskItemsAsync()
    {
        return _taskRepository.GetAllTaskItemsAsync();
    }

    public Task<TaskItem?> GetTaskItemAsync(Guid id)
    {
        return _taskRepository.GetTaskItemAsync(id);
    }

    public Task<TaskItem?> RemoveTaskItemAsync(Guid id)
    {
        return _taskRepository.RemoveTaskItemAsync(id);
    }

    public Task<TaskItem?> UpdateTaskItemAsync(Guid id, UpdateTaskItemRequest request)
    {
        return _taskRepository.UpdateTaskItemAsync(id, request);
    }
}
