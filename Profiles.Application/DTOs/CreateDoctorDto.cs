namespace Profiles.Application.DTOs;

public sealed record CreateDoctorDto(
    string? PhotoPath,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth,
    string Email,
    string Specialization,
    string OfficeAddress,
    int CareerStartYear,
    string Status = "At work"
);

public record DoctorCardDto(
    string? PhotoPath,
    string FullName,
    string Specialization,
    int Experience,
    string OfficeAddress
);