using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        var password = "staff1234";
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);

        Console.WriteLine(Convert.ToBase64String(hash));
    }
}