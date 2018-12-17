using System.Collections.Generic;
using System.Linq;
using SocialApp.DataAccess.Interfaces;
using SocialApp.Domain;

namespace SocialApp.DataAccess
{
    public class SocialAppDataAccess : ISocialAppDataAccess
    {
        private readonly SocialAppDbContext _dbContext;

        public SocialAppDataAccess(SocialAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Value> GetValues()
        {
            return _dbContext.Values.ToList();
        }

        public Value GetValue(int id)
        {
            return _dbContext.Values.FirstOrDefault(x => x.Id == id);
        }
    }
}
