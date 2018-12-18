using SocialApp.Domain;
using System.Threading.Tasks;

namespace SocialApp.Business
{
    public interface IUserManager
    {
        Task<User> Register(string username, string password);
        Task<User> Login(string username, string password);
        object TokenIssuer(User currentUser);
    }
}