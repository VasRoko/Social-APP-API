using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialApp.DataAccess.Interfaces;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;

namespace SocialApp.Business
{
    public class UserManager : IUserManager
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserManager(IAuthRepository authRepository, IConfiguration config, IMapper mapper)
        {
            _authRepository = authRepository;
            _config = config;
            _mapper = mapper;
        }

        public async Task<UserForDetailedDto> Register(UserForRegisterDto userRegister, string password)
        {
            userRegister.Username = userRegister.Username.ToLower();

            if (await _authRepository.UserExists(userRegister.Username))
            {
                return null;
            }
            // Map userForRegister to User class
            var newUser = _mapper.Map<User>(userRegister);

            // Create User
            var createdUser = await _authRepository.Register(newUser, userRegister.Password);

            // Return user for userForList
            return _mapper.Map<UserForDetailedDto>(createdUser);
        }

        public async Task<object> Login(string username, string password)
        {
            var user = await _authRepository.Login(username, password);
            return this.TokenIssuer(_mapper.Map<UserForDetailedDto>(user));
        }

        public object TokenIssuer(UserForDetailedDto user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
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


            return new
            {
                token = tokenHandler.WriteToken(token),
                user
            };
        }
    }
}
