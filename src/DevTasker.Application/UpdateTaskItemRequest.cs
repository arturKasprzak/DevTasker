namespace DevTasker.Application;
public record UpdateTaskItemRequest(
    string Title,
    string? Description,
    bool? Status,
    Guid? UserId
);
