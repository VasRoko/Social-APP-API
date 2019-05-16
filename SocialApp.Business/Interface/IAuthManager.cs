using SocialApp.Domain;
using SocialApp.Domain.Dtos;
using System.Threading.Tasks;

namespace SocialApp.Business.Interface
{
    public interface IAuthManager
    {
        Task<Result> Register(UserForRegisterDto userForRegister, string password);
        Task<object> Login(string username, string password);
        Task<object> TokenIssuer(User currentUser);
    }
}