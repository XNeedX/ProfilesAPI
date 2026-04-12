namespace InnoClinic.Profiles.Api.DTOs;

public sealed record DoctorViewByDoctorResponse(
    string? Photo,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth,
    string Specialization,
    string Office,
    int CareerStartYear
);