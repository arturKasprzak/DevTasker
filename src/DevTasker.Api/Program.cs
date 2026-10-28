using System.ComponentModel.DataAnnotations;

using DevTasker.Application;
using DevTasker.Domain;
using DevTasker.Infrastructure;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<INotificationSender, EmailNotificationSender>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<ITaskRepository, EfTaskRepository>();
builder.Services.AddScoped<TaskAssignedNotificationService>();
builder.Services.AddScoped<ITaskService, TaskService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<DevTaskerDbContext>(options =>
    //options.UseSqlServer(connectionString)
    options.UseNpgsql(connectionString)
    );
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();
}

app.UseCors("AngularClient");
app.MapControllers();

app.MapHealthChecks("/health");

app.MapPost("api/tasks/assign", async (AssignTaskRequest request, TaskAssignedNotificationService service) =>
{
    try
    {
        await service.NotifyTaskAssignedAsync(request.UserId, request.TaskTitle);

        return Results.Ok();
    }
    catch (UserNotFoundException)
    {
        return Results.NotFound();
    }
});

app.MapGet("api/tasks", async (ITaskService service) =>
{
    var tasks = await service.GetAllTaskItemsAsync();

    var response = tasks.Select(t => new TaskResponse(
        t.Id,
        t.Title,
        t.Description,
        t.Status,
        t.AssignedUser?.Name ?? "Unassigned",
        t.AssignedUser?.Id));

    return Results.Ok(response);
});

app.MapGet("api/tasks/{id}", async (Guid id, ITaskService service) =>
{
    try
    {
        var task = await service.GetTaskItemAsync(id);
        if (task is null)
            throw new TaskNotFoundException(id);

        var response = new TaskResponse(
        task.Id,
        task.Title,
        task.Description,
        task.Status,
        task.AssignedUser?.Name ?? "Unassigned",
        task.AssignedUser?.Id);

        return Results.Ok(response);
    }
    catch (TaskNotFoundException)
    {
        return Results.NotFound();
    }
});

app.MapPost("api/tasks", async (CreateTaskRequest request, ITaskService service, IUserRepository userRepo) =>
{
    try
    {
        User? user = null;
        if (request.UserId.HasValue)
        {
            user = await userRepo.GetByIdAsync(request.UserId.Value);
            if (user is null)
                throw new UserNotFoundException(request.UserId.Value);
        }

        var taskItem = new TaskItem(request.Title, request.Description, request.Status, user);
        await service.AddNewTaskItemAsync(taskItem);

        return Results.Created($"/api/tasks/{taskItem.Id}", taskItem.Id);
    }
    catch (UserNotFoundException)
    {
        return Results.NotFound();
    }
});

app.MapDelete("api/tasks/{id}", async (Guid id, ITaskService service) =>
{
    try
    {
        var taskItem = await service.RemoveTaskItemAsync(id);
        if (taskItem is null)
            throw new TaskNotFoundException(id);
        return Results.NoContent();
    }
    catch (TaskNotFoundException)
    {
        return Results.NotFound();
    }
});

app.MapPut("api/tasks/{id}", async (Guid id, UpdateTaskRequest request, ITaskService service) =>
{
    try
    {
        var appRequest = new UpdateTaskItemRequest(request.Title, request.Description, request.Status, request.UserId);
        var taskItem = await service.UpdateTaskItemAsync(id, appRequest);
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

        return Results.Ok(response);
    }
    catch (TaskNotFoundException)
    {
        return Results.NotFound();
    }
    catch (UserNotFoundException)
    {
        return Results.UnprocessableEntity(request.UserId);
    }
});

app.Run();

public record AssignTaskRequest(Guid UserId, string TaskTitle);
public record TaskResponse(Guid Id, string Title, string? Description, bool Status, string UserName, Guid? UserId);
public record CreateTaskRequest([Required] [MaxLength(256)] string Title, string? Description, bool? Status, Guid? UserId);
public record UpdateTaskRequest([Required][MaxLength(256)] string Title, string? Description, bool? Status, Guid? UserId);

public partial class Program { }
