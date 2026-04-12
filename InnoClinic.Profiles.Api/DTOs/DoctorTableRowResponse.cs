namespace InnoClinic.Profiles.Api.DTOs;

public record DoctorTableRowResponse(
    Guid Id,
    string FullName,
    string Specialization,
    string Status,
    DateTime DateOfBirth,
    string OfficeAddress
);