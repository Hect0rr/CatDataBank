using CatDataBank.Model;
using System.Linq;
using System.Collections;

namespace CatDataBank.DataAccess
{
    public interface IUserDataAccess
    {
        User GetUserByEmail(string email);
        bool UserExists(string email);
        void AddUser(User user);
        int Commit();
    }
    public class UserDataAccess : IUserDataAccess
    {
        private AppDbContext _context = new AppDbContext();

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(x => x.Email == email);
        }

        public bool UserExists(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }
    }
}