namespace Profiles.Application.Abstractions;

public static class DoctorErrors
{
    public static readonly Error DuplicateEmail = new(
        "Doctor.DuplicateEmail",
        "User with this email already exists",
        ErrorType.Conflict);

    public static readonly Error NotFound = new(
        "Doctor.NotFound",
        "Doctor profile not found",
        ErrorType.NotFound);
}