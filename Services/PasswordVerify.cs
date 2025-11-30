namespace GameStore.Api.Services;
public static class PasswordVerify
{
    public static bool Check(string rawPassword,string storedHash)
    {
       return BCrypt.Net.BCrypt.Verify(rawPassword,storedHash);
    }
}