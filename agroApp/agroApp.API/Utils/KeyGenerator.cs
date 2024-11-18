using System;
using System.Security.Cryptography;

public class KeyGenerator
{
    public static string GenerateStrongKey(int length = 32)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }

    public static void Main(string[] args)
    {
        string strongKey = GenerateStrongKey();
        Console.WriteLine($"Chave forte gerada: {strongKey}");
    }
}