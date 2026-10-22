using DevTasker.Application;
using DevTasker.Domain;

using Microsoft.AspNetCore.Mvc;

namespace DevTasker.Api.Controllers;

[ApiController]
[Route("api/mvc/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly IUserRepository _userRepository;

    public TasksController(ITaskService taskService, IUserRepository userRepository)
    {
        _taskService = taskService;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskService.GetAllTaskItemsAsync();

        var response = tasks.Select(t => new TaskResponse(
            t.Id,
            t.Title,
            t.Description,
            t.Status,
            t.AssignedUser?.Name ?? "Unassigned",
            t.AssignedUser?.Id));

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var task = await _taskService.GetTaskItemAsync(id);
            if (task is null)
                throw new TaskNotFoundException(id);

            var response = new TaskResponse(
            task.Id,
            task.Title,
            task.Description,
            task.Status,
            task.AssignedUser?.Name ?? "Unassigned",
            task.AssignedUser?.Id);

            return Ok(response);
        }
        catch (TaskNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskRequest request)
    {
        try
        {
            User? user = null;
            if (request.UserId.HasValue)
            {
                user = await _userRepository.GetByIdAsync(request.UserId.Value);
                if (user is null)
                    throw new UserNotFoundException(request.UserId.Value);
            }

            var taskItem = new TaskItem(request.Title, request.Description, request.Status, user);
            await _taskService.AddNewTaskItemAsync(taskItem);

            return CreatedAtAction(nameof(GetById), new { id = taskItem.Id}, taskItem.Id);
        }
        catch (UserNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskRequest request)
    {
        try
        {
            var appRequest = new UpdateTaskItemRequest(request.Title, request.Description, request.Status, request.UserId);
            var taskItem = await _taskService.UpdateTaskItemAsync(id, appRequest);
            if (taskItem is null)
            {
                throw new TaskNotFoundException(id);
            }

            var response = new TaskResponse(
            taskItem.Id,
            taskItem.Title,
            taskItem.Description,
            taskItem.Status,
            taskItem.AssignedUser?.Name ?? "Unassigned",
            taskItem.AssignedUser?.Id);

            return Ok(response);
        }
        catch (TaskNotFoundException)
        {
            return NotFound();
        }
        catch (UserNotFoundException)
        {
            return UnprocessableEntity(request.UserId);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var taskItem = await _taskService.RemoveTaskItemAsync(id);
            if (taskItem is null)
                throw new TaskNotFoundException(id);
            return NoContent();
        }
        catch (TaskNotFoundException)
        {
            return NotFound();
        }
    }
}
