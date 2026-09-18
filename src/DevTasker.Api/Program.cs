using DevTasker.Application;
using DevTasker.Domain;
using DevTasker.Infrastructure;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<INotificationSender, EmailNotificationSender>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<ITaskRepository, EfTaskRepository>();
builder.Services.AddScoped<TaskAssignedNotificationService>();
builder.Services.AddScoped<TaskFetchService>();
builder.Services.AddScoped<TaskCreationService>();


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
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AngularClient");

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

app.MapGet("api/tasks", async (TaskFetchService service) =>
{
    var tasks = await service.GetAllTaskItemsAsync();

    var response = tasks.Select(t => new TaskResponse(
        t.Id,
        t.Title,
        t.Description,
        t.Status,
        t.AssignedUser?.Name ?? "Unassigned"));

    return Results.Ok(response);
});

app.MapGet("api/tasks/{id}", async (Guid id, ITaskRepository repo) =>
{
    try
    {
        var task = await repo.GetTaskItemAsync(id);
        if (task is null)
            throw new TaskNotFoundException(id);

        var response = new TaskResponse(
        task.Id,
        task.Title,
        task.Description,
        task.Status,
        task.AssignedUser?.Name ?? "Unassigned");

        return Results.Ok(response);
    }
    catch (TaskNotFoundException)
    {
        return Results.NotFound();
    }
});

app.MapPost("api/tasks", async (CreateTaskRequest request, TaskCreationService service, IUserRepository userRepo) =>
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

app.Run();

record AssignTaskRequest(Guid UserId, string TaskTitle);

record TaskResponse(Guid Id, string Title, string? Description, bool Status, string UserName);
record CreateTaskRequest(string Title, string? Description, bool? Status, Guid? UserId);