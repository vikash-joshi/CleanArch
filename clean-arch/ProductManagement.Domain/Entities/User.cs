namespace ProductManagement.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    public User(Guid id, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.");
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
    }
}