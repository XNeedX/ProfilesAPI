namespace InnoClinic.Profiles.Api.Abstractions;

public interface IEmailService
{
    Task SendCredentialsAsync(string email, string password);
}