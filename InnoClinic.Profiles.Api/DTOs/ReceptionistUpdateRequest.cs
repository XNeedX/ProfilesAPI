namespace InnoClinic.Profiles.Api.DTOs;

public record ReceptionistUpdateRequest(
    string? Photo,
    string FirstName,
    string LastName,
    string? MiddleName,
    string Email,
    string Office
);
