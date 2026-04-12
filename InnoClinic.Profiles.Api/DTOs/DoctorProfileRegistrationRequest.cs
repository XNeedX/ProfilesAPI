namespace InnoClinic.Profiles.Api.DTOs;

public sealed record DoctorProfileRegistrationRequest(
    string? Photo,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth,
    string Email,
    string Specialization,
    string Office,
    int CareerStartYear,
    string Status = "At work"
);

public record DoctorCardResponse(
    string? Photo,
    string FullName,
    string Specialization,
    int Experience,
    string OfficeAddress
);