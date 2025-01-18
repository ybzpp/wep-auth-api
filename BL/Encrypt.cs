using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WebAuth.BL;

public static class Encrypt
{
    public static string? HashPassword(string? password, string salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2
            (
                password,
                System.Text.Encoding.ASCII.GetBytes(salt),
                KeyDerivationPrf.HMACSHA512,
                5000,
                64)
        );
    }
}