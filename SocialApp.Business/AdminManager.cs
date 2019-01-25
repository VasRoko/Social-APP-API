using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SocialApp.Business.Interface;
using SocialApp.DataAccess.Interfaces;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;

namespace SocialApp.Business
{
    public class AdminManager : IAdminManager
    {
        private readonly IAppDataAccess _dataAccess;
        private readonly UserManager<User> _userManager;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public AdminManager(IAppDataAccess dataAccess, UserManager<User> userManager, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _dataAccess = dataAccess;
            _userManager = userManager;
            _cloudinaryConfig = cloudinaryConfig;
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
             );

            _cloudinary = new Cloudinary(acc);
        }
        public async Task<IEnumerable<object>> GetUsersWithRoles()
        {
            return await _dataAccess.GetUsersWithRoles();
        }

        public async Task<IList<string>> EditRoles(string userName, RoleEditDto roles)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = roles.RoleNames;
            selectedRoles = selectedRoles ?? new string[] {};

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if (!result.Succeeded)
            {
                return null;
            }

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!result.Succeeded)
            {
                return null;
            }

            var newRoles = await _userManager.GetRolesAsync(user);

            return newRoles;
        }

        public async Task<IEnumerable<object>> GetPhotoForModeration()
        {
            return await _dataAccess.GetPhotosForModeration();
        }

        public async Task<bool> ApprovePhoto(int id)
        {
            var photo = await _dataAccess.GetPhoto(id);
            photo.IsApproved = true;

            if (await _dataAccess.SaveAll())
            {
                return true;
            }

            return false;
        }

        public async Task<bool> RejectPhoto(int id)
        {
            var photo = await _dataAccess.GetPhoto(id);

            if (photo.IsMain)
            {
                return false;
            }

            if (photo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photo.PublicId);
                var result = _cloudinary.Destroy(deleteParams);
                if (result.Result == "ok")
                {
                    _dataAccess.DeletePhoto(photo);
                }
            }

            if (photo.PublicId == null)
            {
                _dataAccess.DeletePhoto(photo);
            }

            if (await _dataAccess.SaveAll())
            {
                return true;
            }

            return false;

        }
    }
}
