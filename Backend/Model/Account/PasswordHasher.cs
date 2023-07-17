namespace backend.Model.Account;

using System.Security.Cryptography;
using System.Text;

public class PasswordHasher
{
    public static (string hash, string salt) HashPassword(string password)
    {
        byte[] salt = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        using (var sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + Convert.ToBase64String(salt)));
            string hashedPassword = Convert.ToBase64String(hashedBytes);
            return (hashedPassword, Convert.ToBase64String(salt));
        }
    }

    public static bool VerifyPassword(string password, string hash, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            string hashedPassword = Convert.ToBase64String(hashedBytes);
            return hashedPassword == hash;
        }
    }
}

