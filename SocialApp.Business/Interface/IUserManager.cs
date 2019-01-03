using SocialApp.Domain;
using SocialApp.Domain.Dtos;
using System.Threading.Tasks;

namespace SocialApp.Business
{
    public interface IUserManager
    {
        Task<UserForDetailedDto> Register(UserForRegisterDto userForRegister, string password);
        Task<object> Login(string username, string password);
        object TokenIssuer(UserForDetailedDto currentUser);
    }
}