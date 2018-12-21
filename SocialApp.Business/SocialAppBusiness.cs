using System.Collections.Generic;
using System.Threading.Tasks;
using SocialApp.Business.Interface;
using SocialApp.DataAccess.Interfaces;
using AutoMapper;
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

        public async Task<IEnumerable<UserForListDto>> GetUsers()
        {
            var users = await _dataAccess.GetUsers();
            return _mapper.Map<IEnumerable<UserForListDto>>(users);
        }

        public async Task<UserForDetailedDto> GetUser(int id)
        {
            var user = await _dataAccess.GetUser(id);
            return _mapper.Map<UserForDetailedDto>(user);
        }

    }
}
