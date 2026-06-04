namespace Profiles.Application.Abstractions;

public static class PatientErrors
{
    public static readonly Error NotFound = new(
        "Patient.NotFound",
        "Patient profile not found.",
        ErrorType.NotFound);

    public static readonly Error AlreadyLinked = new(
        "Patient.AlreadyLinked",
        "A patient with this personal data is already registered in the system.",
        ErrorType.Conflict);
}