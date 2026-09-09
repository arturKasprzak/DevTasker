
using DevTasker.Domain;

namespace DevTasker.Application;
public class TaskFetchService
{
    private readonly ITaskRepository _taskRepository;

    public TaskFetchService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public Task<IReadOnlyList<TaskItem>> GetAllTaskItemsAsync()
    {
        return _taskRepository.GetAllTaskItemsAsync();
    }
}
