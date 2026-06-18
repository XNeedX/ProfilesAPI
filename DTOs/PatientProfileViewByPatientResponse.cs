namespace InnoClinic.Profiles.Api.DTOs;

public record PatientProfileViewByPatientResponse(
    string? Photo,
    string FirstName,
    string LastName,
    string? MiddleName,
    string PhoneNumber,
    DateTime DateOfBirth
);