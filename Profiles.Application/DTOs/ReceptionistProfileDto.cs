namespace Profiles.Application.DTOs;

public record ReceptionistProfileDto(
    string? PhotoPath,
    string? FirstName,
    string? LastName,
    string? MiddleName,
    string? OfficeAddress
);