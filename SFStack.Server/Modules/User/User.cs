namespace SFStack.Server.Modules;

public class User
{
    public int Id { get; set; }

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}