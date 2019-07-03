using CatDataBank.DataAccess;
using System.Collections.Generic;
using CatDataBank.Model;

namespace CatDataBank.Service
{
    public interface ICatService
    {
        IEnumerable<Cat> GetCats();
        void AddCats(Cat[] cats);
    }
    public class CatService : ICatService
    {
        private readonly ICatDataAccess _catDataAccess;
        public CatService(ICatDataAccess catDataAccess)
        {
            _catDataAccess = catDataAccess;
        }

        public IEnumerable<Cat> GetCats()
        {
            return _catDataAccess.GetAllCats();
        }

        public void AddCats(Cat[] cats)
        {
            _catDataAccess.AddCats(cats);
            _catDataAccess.Commit();
        }
    }
}