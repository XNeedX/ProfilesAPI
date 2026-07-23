namespace Profiles.Application.Abstractions;

public interface IEmailService
{
    Task SendCredentialsAsync(string email, string password);
}