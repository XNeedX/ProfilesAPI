using Profiles.Application.Abstractions;

namespace Profiles.Application.Services;

public class TempEmailService : IEmailService
{
    public Task SendCredentialsAsync(string email, string password)
    {
        // This is a temporary implementation
        Console.WriteLine($"Sending credentials to {email}:");
        Console.WriteLine($"Password: {password}");
        return Task.CompletedTask;
    }
}
