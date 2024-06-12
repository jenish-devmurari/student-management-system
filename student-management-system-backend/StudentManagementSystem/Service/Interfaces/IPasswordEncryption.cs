using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IPasswordEncryption
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string enteredPassword, string storedHashedPassword);
    }
}
