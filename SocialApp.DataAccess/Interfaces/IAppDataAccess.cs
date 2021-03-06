﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SocialApp.Domain;

namespace SocialApp.DataAccess.Interfaces
{
    public interface IAppDataAccess
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void DeletePhoto(Photo photo);
        Task<bool> SaveAll();
        Task<PageList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id, bool isCurrentUser);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhoto(int userId);
        Task<Like> GetLike(int userId, int recipientId);
        Task<Message> GetMessage(int id);
        Task<PageList<Message>> GetMessageForUser(MessageParams MessageParams);
        Task<IEnumerable<Message>> GetMessageThred(int userId, int recipientId);
        Task<IEnumerable<object>> GetUsersWithRoles();
        Task<IEnumerable<object>> GetPhotosForModeration();
    }
}