

using DevTasker.Domain;

namespace DevTasker.Application;
public class TaskCreationService
{
    private readonly ITaskRepository _taskRepository;

    public TaskCreationService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task AddNewTaskItemAsync(TaskItem item)
    {
        await _taskRepository.AddNewTaskItemAsync(item);
    }
}
