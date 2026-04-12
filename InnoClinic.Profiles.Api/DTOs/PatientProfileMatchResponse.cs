namespace InnoClinic.Profiles.Api.DTOs;

public record PatientProfileMatchResponse(
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