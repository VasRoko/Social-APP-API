using System.Threading.Tasks;
using SocialApp.Domain.Dtos;
using SocialApp.Domain;

namespace SocialApp.Business.Interface
{
    public interface IPhotoManager
    {
        Task<PhotoForReturnDto> AddPhotoUser(int userId, PhotoForCreationDto photoForCreationDto);
        Task<PhotoForReturnDto> GetPhoto(int id);
    }
}