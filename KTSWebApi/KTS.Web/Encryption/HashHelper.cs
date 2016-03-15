using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTS.Web.Encryption
{
    public static class HashHelper
    {
        public static string GenerateHashedString(string valueToHash)
        {
            return BCrypt.Net.BCrypt.HashPassword(valueToHash, 12);
        }

        public static bool ValidateHashedValue(string hashedValue, string unHashedValue)
        {
            return BCrypt.Net.BCrypt.Verify(unHashedValue, hashedValue);
        }
    }
}
