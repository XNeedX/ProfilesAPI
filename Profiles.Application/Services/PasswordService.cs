using System.Security.Cryptography;
using Profiles.Application.Abstractions;

namespace Profiles.Application.Services;

internal class PasswordService : IPasswordGenerator
{
    public string GeneratePassword()
    {
        var alphanumeric = RandomNumberGenerator.GetString(
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 15);

        var nonAlphanumeric = RandomNumberGenerator.GetString(
            "!@#$%^&*()_-+=[{]};:>|./?", 10);

        var combined = alphanumeric + nonAlphanumeric;

        var shuffledPassword = combined.OrderBy(x => Guid.NewGuid()).ToArray();

        return new string(shuffledPassword);
    }
}