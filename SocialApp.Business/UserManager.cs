using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialApp.DataAccess.Interfaces;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;

namespace SocialApp.Business
{
    public class UserManager : IUserManager
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        private readonly UserManager<User> _userIdentityManager;
        private readonly SignInManager<User> _signInManager;

        public UserManager(
            IConfiguration config,
            IMapper mapper,
            UserManager<User> userIdentityManager,
            SignInManager<User> signInManager
            )
        {
            _config = config;
            _mapper = mapper;
            _userIdentityManager = userIdentityManager;
            _signInManager = signInManager;
        }

        public async Task<UserForDetailedDto> Register(UserForRegisterDto userRegister, string password)
        {
            var newUser = _mapper.Map<User>(userRegister);
            var result = await _userIdentityManager.CreateAsync(newUser, userRegister.Password);

            if (result.Succeeded)
            {
                // Return user for userForList
                return _mapper.Map<UserForDetailedDto>(newUser);
            }

            return null;
        }

        public async Task<object> Login(string username, string password)
        {
            var user = await _userIdentityManager.FindByNameAsync(username);
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
            {
                var appUser = await _userIdentityManager.Users.Include(p => p.Photos)
                    .FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());

                return this.TokenIssuer(_mapper.Map<UserForDetailedDto>(appUser));
            }

            return null;

        }

        public object TokenIssuer(UserForDetailedDto user)
        {
            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

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

            return null;
        }
    }
}
