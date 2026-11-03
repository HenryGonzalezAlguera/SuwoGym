using System.Security.Cryptography;

namespace SuwoGym_API.Services;

/// <summary>
/// Genera credenciales aleatorias solo para recuperación temporal de cuentas.
/// Nunca almacena ni registra la contraseña en texto plano.
/// </summary>
public static class GeneradorClavesTemporales
{
    private const string Caracteres =
        "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@#$%";

    public static string Generar()
    {
        var caracteres = new char[24];
        for (var i = 0; i < caracteres.Length; i++)
        {
            caracteres[i] = Caracteres[
                RandomNumberGenerator.GetInt32(Caracteres.Length)];
        }
        return new string(caracteres);
    }
}
