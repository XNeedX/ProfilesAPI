namespace Profiles.Application.DTOs;

public sealed record DoctorAdminDto(
    string? PhotoPath,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth,
    string Specialization,
    string OfficeAddress,
    int CareerStartYear,
    string Status
);