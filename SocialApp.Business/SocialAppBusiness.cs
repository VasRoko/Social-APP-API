using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using SocialApp.Business.Interface;
using SocialApp.DataAccess.Interfaces;
using AutoMapper;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;

namespace SocialApp.Business
{
    public class SocialAppBusiness : ISocialAppBusiness
    {
        private readonly ISocialAppDataAccess _dataAccess;
        private readonly IMapper _mapper;

        public SocialAppBusiness(ISocialAppDataAccess dataAccess, IMapper mapper)
        {
            _dataAccess = dataAccess;
            _mapper = mapper;
        }

        public async Task<PageList<User>> GetUsers(UserParams userParams)
        {
            return await _dataAccess.GetUsers(userParams);
        }

        public async Task<UserForDetailedDto> GetUser(int id)
        {
            var user = await _dataAccess.GetUser(id);
            return _mapper.Map<UserForDetailedDto>(user);
        }

        public IEnumerable<UserForListDto> Users(PageList<User> users)
        {
            return _mapper.Map<IEnumerable<UserForListDto>>(users);
        }

        public async Task<UserForUpdateDto> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            var userFromContext = await _dataAccess.GetUser(id);
            _mapper.Map(userForUpdateDto, userFromContext);

            if (await _dataAccess.SaveAll())
            {
                return userForUpdateDto;
            }

            return null;
        }

    }
}
