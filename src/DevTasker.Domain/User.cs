namespace DevTasker.Domain;

public class User
{
    public User(string email, string name)
    {
        Id = Guid.NewGuid();
        ChangeEmail(email);
        ChangeName(name);
    }
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Name { get; private set; }

    public void ChangeEmail(string newEmail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newEmail);
        Email = newEmail;
    }
    public void ChangeName(string newName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);
        Name = newName;
    }
}