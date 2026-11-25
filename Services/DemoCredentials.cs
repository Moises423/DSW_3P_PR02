namespace DSW_3P_PR02.Services;

public sealed class DemoCredentials(string username, string password)
{
    public string Username { get; } = username;

    public string Password { get; } = password;
}
