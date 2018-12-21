using System.Collections.Generic;
using System.Threading.Tasks;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;

namespace SocialApp.Business.Interface
{
    public interface ISocialAppBusiness
    {
        Task<IEnumerable<UserForListDto>> GetUsers();
        Task<UserForDetailedDto> GetUser(int id);
        Task<UserForUpdateDto> UpdateUser(int id, UserForUpdateDto userForUpdateDto);
    }
}