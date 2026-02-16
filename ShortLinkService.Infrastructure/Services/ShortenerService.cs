using System.Security.Cryptography;
using ShortLinkService.Application.Common;

namespace ShortLinkService.Infrastructure.Services;

public class ShortenerService : IShortenerService
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int CodeLength = 7;

    public string GenerateCode()
    {
        var chars = new char[CodeLength];
        var bytes = RandomNumberGenerator.GetBytes(CodeLength);

        for (var i = 0; i < CodeLength; i++)
        {
            chars[i] = Alphabet[bytes[i] % Alphabet.Length];
        }

        return new string(chars);
    }
}