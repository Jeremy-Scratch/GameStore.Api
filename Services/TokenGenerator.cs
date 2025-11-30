using System.Security.Cryptography;

namespace GameStore.Api.Services;

public static class TokenGenerator
{
    public static async Task<string> GenerateToken()
    {
        var randomNumber = new byte[32];

        using( var rgn = RandomNumberGenerator.Create())
        {
            rgn.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }

}
