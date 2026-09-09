using DevTasker.Application;
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

app.Run();

record AssignTaskRequest(Guid UserId, string TaskTitle);

record TaskResponse(Guid Id, string Title, string? Description, bool Status, string UserName);