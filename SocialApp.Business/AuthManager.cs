using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialApp.Business.Interface;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;

namespace SocialApp.Business
{
    public class AuthManager : IAuthManager
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        private readonly UserManager<User> _userIdentityManager;
        private readonly SignInManager<User> _signInManager;

        public AuthManager(
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

        public async Task<Result> Register(UserForRegisterDto userRegister, string password)
        {
            Result result = new Result();
            var newUser = _mapper.Map<User>(userRegister);
            var res = await _userIdentityManager.CreateAsync(newUser, userRegister.Password);


            if (res.Succeeded)
            {
                result.isValid = true;
                result.Data = _mapper.Map<UserForRegisterDto>(newUser);
            }
            else
            {
                result.Message = res.ToString();
            }

            return result;
        }

        public async Task<object> Login(string username, string password)
        {
            SignInResult result = new SignInResult();
            var user = await _userIdentityManager.FindByNameAsync(username);
            if (user != null)
            {
                result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            }

            if (result.Succeeded)
            {
                var appUser = await _userIdentityManager.Users.Include(p => p.Photos)
                    .FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());
                var userToken = await this.TokenIssuer(_mapper.Map<User>(appUser));

                return userToken;
            }

            return null;

        }

        public async Task<object> TokenIssuer(User _user)
        {
            if (_user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString()),
                    new Claim(ClaimTypes.Name, _user.UserName)
                };

                var roles = await _userIdentityManager.GetRolesAsync(_user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

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
                var user = _mapper.Map<UserForDetailedDto>(_user);

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
