namespace InnoClinic.Profiles.Api.DTOs;

public record ReceptionistViewRequest(
    string? Photo,
    string? FirstName,
    string? LastName,
    string? MiddleName,
    string? Office
);