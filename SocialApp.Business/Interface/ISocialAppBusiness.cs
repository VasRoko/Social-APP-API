using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;

namespace SocialApp.Business.Interface
{
    public interface ISocialAppBusiness
    {
        IEnumerable<UserForListDto> Users(PageList<User> users);
        Task<PageList<User>> GetUsers(UserParams userParams, int userId);
        Task<UserForDetailedDto> GetUser(int id);
        Task<UserForUpdateDto> UpdateUser(int id, UserForUpdateDto userForUpdateDto);
    }
}