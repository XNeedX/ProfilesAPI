namespace Profiles.Application.DTOs;

public record CreatePatientDto(
    string? PhotoPath,
    string FirstName,
    string LastName,
    string? MiddleName,
    string PhoneNumber,
    string Email,
    DateTime DateOfBirth,
    bool IsEmailVerified,
    string AccountId
);