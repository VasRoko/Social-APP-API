using System.Threading.Tasks;
using SocialApp.DataAccess.Interfaces;
using SocialApp.Domain;

namespace SocialApp.Business
{
    public class UserManager : IUserManager
    {
        private readonly IAuthRepository _authRepository;

        public UserManager(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<User> Register(string username, string password)
        {
            username = username.ToLower();

            if (await _authRepository.UserExists(username))
            {
                return null;
            }

            var newUser = new User
            {
                Username = username
            };

            var createdUser = await _authRepository.Register(newUser, password);

            return createdUser;
        }
    }
}
