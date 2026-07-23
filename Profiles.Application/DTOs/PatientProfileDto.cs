namespace Profiles.Application.DTOs;

public record PatientProfileDto(
    string? PhotoPath,
    string FirstName,
    string LastName,
    string? MiddleName,
    string PhoneNumber,
    DateTime DateOfBirth
);