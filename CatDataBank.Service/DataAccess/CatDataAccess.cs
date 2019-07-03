using CatDataBank.Model;
using System.Collections.Generic;

namespace CatDataBank.DataAccess
{
    public interface ICatDataAccess
    {
        IEnumerable<Cat> GetAllCats();
        void AddCats(IEnumerable<Cat> cats);
        int Commit();
    }
    public class CatDataAccess : ICatDataAccess
    {
        private AppDbContext _context = new AppDbContext();

        public IEnumerable<Cat> GetAllCats()
        {
            return _context.Cats;
        }

        public void AddCats(IEnumerable<Cat> cats)
        {
            _context.AddRange(cats);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }
    }
}