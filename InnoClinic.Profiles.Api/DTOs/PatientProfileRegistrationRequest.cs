namespace InnoClinic.Profiles.Api.DTOs;

public record PatientProfileRegistrationRequest(
    string? Photo,
    string FirstName,
    string LastName,
    string? MiddleName,
    string PhoneNumber,
    DateTime DateOfBirth,
    bool IsEmailVerified,
    string AccountId
);