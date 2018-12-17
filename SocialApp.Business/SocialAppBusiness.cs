using System.Collections.Generic;
using SocialApp.Business.Interface;
using SocialApp.DataAccess.Interfaces;
using SocialApp.Domain;

namespace SocialApp.Business
{
    public class SocialAppBusiness : ISocialAppBusiness
    {
        private readonly ISocialAppDataAccess _dataAccess;

        public SocialAppBusiness(ISocialAppDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public List<Value> BusinessTest()
        {
            return _dataAccess.GetValues();
        }

        public Value GetValue(int id)
        {
            return _dataAccess.GetValue(id);
        }
    }
}
