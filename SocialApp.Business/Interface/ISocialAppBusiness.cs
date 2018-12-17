using System.Collections.Generic;
using SocialApp.Domain;

namespace SocialApp.Business.Interface
{
    public interface ISocialAppBusiness
    {
        List<Value> BusinessTest();
        Value GetValue(int id);
    }
}