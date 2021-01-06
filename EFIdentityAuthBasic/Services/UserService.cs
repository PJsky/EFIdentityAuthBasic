using EFIdentityAuthBasic.DatabaseContext;
using EFIdentityAuthBasic.Helpers;
using EFIdentityAuthBasic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFIdentityAuthBasic.Services
{
    public class UserService : IUserService
    {
        private IDbContext context;
        private PasswordHasher pHasher = new PasswordHasher();

        public UserService(IDbContext Context)
        {
            context = Context;
        }

        public User Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return null;

            User user = context.Users.Where(u => u.Email == email).FirstOrDefault();

            if (user == null) return null;

            //If password hasher wont verify the validity of password return nothing
            if(!pHasher.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;

            return user;
        }

        public User Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new Exception("Password cannot be null or whitespace only");
            if (context.Users.Any(u => u.Email == user.Email)) throw new Exception("Username already taken");

            byte[] passwordHash, passwordSalt;
            pHasher.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            context.Users.Add(user);
            context.SaveChanges();

            return user;
        }

        public User GetById(int id) => context.Users.Find(id);
    }
}
