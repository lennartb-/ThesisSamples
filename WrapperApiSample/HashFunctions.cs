using System.Security.Cryptography;
using System.Text;

namespace WrapperApi;

/// <summary>
///     Provides sample hashing functions.
/// </summary>
public static class HashFunctions
{
    /// <summary>
    ///     Hashes a string using the SHA512 algorithm.
    /// </summary>
    /// <param name="input">An UTF8 string to hash.</param>
    /// <returns>
    ///     The SHA512-hashed equivalent of <paramref name="input" />,
    ///     as a uppercase hex string without dashes.
    /// </returns>
    public static string HashAsSha512(this string input)
    {
        using var sha512Hash = SHA512.Create();
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashedBytes = sha512Hash.ComputeHash(inputBytes);
        return Convert.ToHexString(hashedBytes);
    }

    /// <summary>
    ///     Hashes a string using the SHA256 algorithm.
    /// </summary>
    /// <param name="input">An UTF8 string to hash.</param>
    /// <returns>
    ///     The SHA256-hashed equivalent of <paramref name="input" />,
    ///     as a uppercase hex string without dashes.
    /// </returns>
    public static string HashAsSha256(this string input)
    {
        using var sha256Hash = SHA256.Create();
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashedBytes = sha256Hash.ComputeHash(inputBytes);
        return Convert.ToHexString(hashedBytes);
    }

    /// <summary>
    ///     Hashes a string using the MD5 algorithm.
    /// </summary>
    /// <param name="input">An UTF8 string to hash.</param>
    /// <returns>
    ///     The MD5-hashed equivalent of <paramref name="input" />,
    ///     as a uppercase hex string without dashes.
    /// </returns>
    public static string HashAsMd5(this string input)
    {
        using var md5Hash = MD5.Create();
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashedBytes = md5Hash.ComputeHash(inputBytes);
        return Convert.ToHexString(hashedBytes);
    }

    /// <summary>
    ///     Hashes a string using the SHA1 algorithm.
    /// </summary>
    /// <param name="input">An UTF8 string to hash.</param>
    /// <returns>
    ///     The SHA1-hashed equivalent of <paramref name="input" />,
    ///     as a uppercase hex string without dashes.
    /// </returns>
    public static string HashAsSha1(this string input)
    {
        using var sha1Hash = SHA1.Create();
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashedBytes = sha1Hash.ComputeHash(inputBytes);
        return Convert.ToHexString(hashedBytes);
    }
}