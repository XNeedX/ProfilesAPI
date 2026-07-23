namespace Profiles.Application.Abstractions;

public static class ReceptionistErrors
{
    public static readonly Error NotFound = new(
        "Receptionist.NotFound",
        "Receptionist profile not found.",
        ErrorType.NotFound);

    public static readonly Error DuplicateEmail = new(
        "Receptionist.DuplicateEmail",
        "User with this email already exists.",
        ErrorType.Conflict);
}