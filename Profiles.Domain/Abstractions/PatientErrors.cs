namespace Profiles.Domain.Abstractions;

public static class PatientErrors
{
    public static readonly Error NotFound = new("Patient.NotFound", "Patient profile not found.");
    public static readonly Error AlreadyLinked = new("Patient.AlreadyLinked", "A patient with this personal data is already registered in the system.");
}