using System.Collections.Generic;
using System.Threading.Tasks;
using SocialApp.Business.Interface;
using SocialApp.DataAccess.Interfaces;
using AutoMapper;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;

namespace SocialApp.Business
{
    public class AppBusiness : IAppBusiness
    {
        private readonly IAppDataAccess _dataAccess;
        private readonly IMapper _mapper;

        public AppBusiness(IAppDataAccess dataAccess, IMapper mapper)
        {
            _dataAccess = dataAccess;
            _mapper = mapper;
        }

        public async Task<PageList<User>> GetUsers(UserParams userParams, int userId)
        {
            var currentUser = await _dataAccess.GetUser(userId);
            userParams.UserId = currentUser.Id;
            var users = await _dataAccess.GetUsers(userParams);
            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
            }
            
            return await _dataAccess.GetUsers(userParams);
        }

        public IEnumerable<UserForListDto> MapUsers(int userid, PageList<User> pagination)
        {
            var users = _mapper.Map<IEnumerable<UserForListDto>>(pagination);
            Task<IEnumerable<UserForListDto>> returnUsers = Task.Run<IEnumerable<UserForListDto>>(async () => await SetIsLikedAsync(userid, users));
            return returnUsers.Result;
        }

        private async Task<IEnumerable<UserForListDto>> SetIsLikedAsync(int userid, IEnumerable<UserForListDto> users)
        {
            foreach (var user in users)
            {
                if (await _dataAccess.GetLike(userid, user.Id) != null)
                {
                    user.IsLiked = true;
                }
            }

            return users;
        }

        public async Task<UserForDetailedDto> GetUser(int currentUserid, int id)
        {
            var user = await _dataAccess.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            if(await _dataAccess.GetLike(currentUserid, user.Id) != null)
            {
                userToReturn.IsLiked = true;
            }
            return userToReturn;
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

        public async Task<string> Like(int userId, int recipientId)
        {
            var like = await _dataAccess.GetLike(userId, recipientId);

            if (like != null)
            {
                _dataAccess.Delete(like);
                if (await _dataAccess.SaveAll())
                {
                    return "Ok";
                }
            }

            if (await _dataAccess.GetUser(recipientId) == null)
            {
                return null;
            }

            like = new Like
            {
                LikerId = userId,
                LikeeId = recipientId
            };

            _dataAccess.Add<Like>(like);

            if (await _dataAccess.SaveAll())
            {
                return "Ok";
            }

            return "Failed to Like User";
        }
    }
}
