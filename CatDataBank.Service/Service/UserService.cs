using System;
using System.Runtime.CompilerServices;
using CatDataBank.DataAccess;
using CatDataBank.Helper;
using CatDataBank.Model;
using Microsoft.AspNetCore.Http;

[assembly : InternalsVisibleTo("CatDataBank.Test")]
[assembly : InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace CatDataBank.Service
{
    public interface IUserService
    {
        string Authenticate(string username, string password);
        User Create(User user, string password);
    }

    public class UserService : IUserService
    {
        private readonly IUserDataAccess _userDataAccess;
        private readonly ITokenHandler _tokenHandler;
        public UserService(IUserDataAccess userDataAccess, ITokenHandler tokenHandler)
        {
            _userDataAccess = userDataAccess;
            _tokenHandler = tokenHandler;
        }

        public String Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = _userDataAccess.GetUserByEmail(email);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return _tokenHandler.GenerateToken(user.Id);
        }

        public User Create(User user, string password)
        {
            if (user == null || string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            if (_userDataAccess.UserExists(user.Email))
                throw new Exception($"Username {user.Email} is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _userDataAccess.AddUser(user);
            _userDataAccess.Commit();

            return user;
        }

        internal virtual void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        internal virtual bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Le mot de passe ne peut pas être null");
            if (storedHash.Length != 64) throw new ArgumentException("Longueur de mot de passe hashé invalide");
            if (storedSalt.Length != 128) throw new ArgumentException("Longueur de mot de passe salé invalide");

            using(var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}