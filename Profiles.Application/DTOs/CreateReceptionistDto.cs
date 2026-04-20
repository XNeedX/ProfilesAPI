namespace Profiles.Application.DTOs;

public record CreateReceptionistRequest(
    string? PhotoPath,
    string FirstName,
    string LastName,
    string? MiddleName,
    string Email,
    string OfficeAddress
);