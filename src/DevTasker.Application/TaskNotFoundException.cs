namespace DevTasker.Application;
public class TaskNotFoundException: Exception
{
    public TaskNotFoundException(Guid taskId) : base($"Task with id: {taskId} was not found")
    {
        
    }
}
