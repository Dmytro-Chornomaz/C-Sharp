using System.Security.Cryptography;
using System.Text;

namespace Finance_Organizer
{
    public class SaltedHash
    {
        string salt = "Fr721*J3W_Sa";

        public string ComputeSaltedHash(string password)
        {
            string passwordAndSalt = password + salt;

            byte[] passwordAndSaltBytes = Encoding.UTF8.GetBytes(passwordAndSalt);

            SHA256 hash = SHA256.Create();

            byte[] hashBytes = hash.ComputeHash(passwordAndSaltBytes);

            string hashValue = Convert.ToBase64String(hashBytes);

            return hashValue;
        }
    }
}
