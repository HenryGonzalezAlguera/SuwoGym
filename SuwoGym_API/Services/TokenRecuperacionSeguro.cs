using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;

namespace SuwoGym_API.Services;

/// <summary>Token aleatorio de 256 bits. Solo se almacena su SHA-256 en SQL.</summary>
public static class TokenRecuperacionSeguro
{
    public static (string Token, byte[] Hash) Crear()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return (WebEncoders.Base64UrlEncode(bytes), SHA256.HashData(bytes));
    }

    public static bool TryObtenerHash(string? token, out byte[] hash)
    {
        hash = Array.Empty<byte>();
        if (string.IsNullOrWhiteSpace(token) || token.Length != 43)
            return false;

        try
        {
            var bytes = WebEncoders.Base64UrlDecode(token);
            if (bytes.Length != 32)
                return false;

            hash = SHA256.HashData(bytes);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
