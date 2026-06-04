namespace Profiles.Application.DTOs;

public record DoctorTableRowDto(
    Guid Id,
    string FullName,
    string Specialization,
    string Status,
    DateTime DateOfBirth,
    string OfficeAddress
);