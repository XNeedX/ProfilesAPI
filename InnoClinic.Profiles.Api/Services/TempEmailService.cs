using InnoClinic.Profiles.Api.Abstractions;

namespace InnoClinic.Profiles.Api.Services;

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
