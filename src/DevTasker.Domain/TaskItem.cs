
namespace DevTasker.Domain;
public class TaskItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public bool Status { get; private set; }
    public User? AssignedUser { get; private set; }

    public TaskItem(string title, string? description, bool? status, User? assignedUser)
    {
        Id = Guid.NewGuid();
        ChangeTitle(title);
        ChangeDescription(description);
        if(assignedUser != null)
            AssignTo(assignedUser);
        if (status == true)
            MarkAsCompleted();
    }

    public void AssignTo(User? assignedUser)
    {
        AssignedUser = assignedUser;
    }

    public void ChangeDescription(string? description)
    {
        Description = description;
    }

    public void ChangeTitle(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        Title = title;
    }

    public void MarkAsCompleted()
    {
        Status = true;
    }
    public void MarkAsIncomplete()
    {
        Status = false;
    }
}
