namespace InnoClinic.Profiles.Api.DTOs;

public record CreateReceptionistRequest(
    string? Photo,
    string FirstName,
    string LastName,
    string? MiddleName,
    string Email,
    string Office
);