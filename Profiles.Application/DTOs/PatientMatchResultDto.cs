namespace Profiles.Application.DTOs;

public record PatientMatchResultDto(
    bool IsMatchFound,
    string Message,
    ExistingProfileDto? ExistingProfile
);

public record ExistingProfileDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth
);