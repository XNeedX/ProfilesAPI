namespace InnoClinic.Profiles.Api.DTOs;

public sealed record DoctorViewByAdminResponse(
    string? Photo,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth,
    string Specialization,
    string Office,
    int CareerStartYear,
    string status
);