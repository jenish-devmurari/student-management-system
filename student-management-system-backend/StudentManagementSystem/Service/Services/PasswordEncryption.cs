using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class PasswordEncryption : IPasswordEncryption
    {
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedPasswordBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedPasswordBytes);
            }
        }

        public bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {

            string hashedEnteredPassword = HashPassword(enteredPassword);

            return hashedEnteredPassword.Equals(storedHashedPassword);
        }
    }
}
