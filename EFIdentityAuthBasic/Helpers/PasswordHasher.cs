using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EFIdentityAuthBasic.Helpers
{
    internal class PasswordHasher
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new Exception("Password cannot be null or whitespace only");
            
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new Exception("Password cannot be null or whitespace only");
            if (passwordHash.Length != 64) throw new ArgumentException("Invalid hash length");
            if (passwordSalt.Length != 128) throw new ArgumentException("Invalid salt length");

            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                
                for (int i = 0; i < computedHash.Length; i++)
                    if (computedHash[i] != passwordHash[i]) return false;
            }

            return true;
        }
    }
}
