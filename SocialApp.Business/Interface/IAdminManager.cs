using SocialApp.Domain.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialApp.Business.Interface
{
    public interface IAdminManager
    {
        Task<IEnumerable<object>> GetUsersWithRoles();
        Task<IEnumerable<object>> GetPhotoForModeration();
        Task<bool> ApprovePhoto(int id);
        Task<bool> RejectPhoto(int id);
        Task<IList<string>> EditRoles(string userName, RoleEditDto roleEditDto);
    }
}