namespace Profiles.Domain.Abstractions;

public static class ReceptionistErrors
{
    public static readonly Error NotFound = new("Receptionist.NotFound", "Receptionist profile not found.");
    public static readonly Error DuplicateEmail = new("Receptionist.DuplicateEmail", "User with this email already exists.");
}