using System.Security.Cryptography;

namespace VebtechTestTask.Application.Helpers;

public static class PasswordHelper
{
    private const int COUNT_OF_ITERATIONS = 100_000;
    private const int COUNT_OF_BYTES_IN_PASSWORD = 20;

    public static (byte[] Password, byte[] Salt) HashPassword(string password)
    {
        byte[] salt = RNGCryptoServiceProvider.GetBytes(16);
        var t = System.Text.Encoding.UTF8.GetString(salt);
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, COUNT_OF_ITERATIONS);
        var passwordHash = pbkdf2.GetBytes(COUNT_OF_BYTES_IN_PASSWORD);
        var t1 = System.Text.Encoding.UTF8.GetString(passwordHash);
        return (passwordHash, salt);
    }

    public static bool ValidatePassword(string password, byte[] realPasswordHash, byte[] salt)
    {
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, COUNT_OF_ITERATIONS);
        var passwordHash = pbkdf2.GetBytes(COUNT_OF_BYTES_IN_PASSWORD);
        return realPasswordHash.SequenceEqual(passwordHash);
    }
}
