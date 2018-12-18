using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialApp.DataAccess.Interfaces;
using SocialApp.Domain;

namespace SocialApp.Business
{
    public class UserManager : IUserManager
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;

        public UserManager(IAuthRepository authRepository, IConfiguration config)
        {
            _authRepository = authRepository;
            _config = config;
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

        public async Task<User> Login(string username, string password)
        {
            return await _authRepository.Login(username, password);
        }

        public object TokenIssuer(User currentUser)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, currentUser.Id.ToString()),
                new Claim(ClaimTypes.Name, currentUser.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var createdToken = tokenHandler.WriteToken(token);

            return createdToken;
        }
    }
}
