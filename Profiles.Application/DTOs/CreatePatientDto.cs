namespace Profiles.Application.DTOs;

public record CreatePatientDto(
    string? PhotoPath,
    string FirstName,
    string LastName,
    string? MiddleName,
    string PhoneNumber,
    DateTime DateOfBirth,
    bool IsEmailVerified,
    string AccountId
);