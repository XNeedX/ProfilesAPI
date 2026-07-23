namespace Profiles.Application.DTOs;

public record UpdateReceptionistDto(
    string? PhotoPath,
    string FirstName,
    string LastName,
    string? MiddleName,
    string Email,
    string OfficeAddress
);
