using System.Collections.Generic;
using SocialApp.Domain;

namespace SocialApp.DataAccess.Interfaces
{
    public interface ISocialAppDataAccess
    {
        List<Value> GetValues();
        Value GetValue(int id);
    }
}