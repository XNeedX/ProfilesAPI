namespace Profiles.Application.DTOs;

public record DoctorProfileDto(
    string? PhotoPath,
    string FirstName,
    string LastName,
    string? MiddleName,
    string PhoneNumber,
    DateTime DateOfBirth
);