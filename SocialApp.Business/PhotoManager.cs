﻿using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using SocialApp.Business.Interface;
using SocialApp.DataAccess.Interfaces;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;

namespace SocialApp.Business
{
    public class PhotoManager : IPhotoManager
    {
        private readonly IAppDataAccess _dataContext;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly IMapper _mapper;
        private Cloudinary _cloudinary;
        private Result _result;

        public PhotoManager(IAppDataAccess dataContext, IOptions<CloudinarySettings> cloudinaryConfig, IMapper mapper)
        {
            _dataContext = dataContext;
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
            _result = new Result();
        }

        public async Task<PhotoForReturnDto> AddPhotoUser(int userId, PhotoForCreationDto photoForCreationDto)
        {

            var user = await _dataContext.GetUser(userId, true);
            var uploadResult = new ImageUploadResult();
            var file = photoForCreationDto.File;

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                };
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            if (!user.Photos.Any(u => u.IsMain))
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _dataContext.SaveAll())
            {
                return _mapper.Map<PhotoForReturnDto>(photo);
            }

            return null;
        }

        public async Task<PhotoForReturnDto> GetPhoto(int id)
        {
            var photo = await _dataContext.GetPhoto(id);
            return _mapper.Map<PhotoForReturnDto>(photo);
        }

        public async Task<Result> SetMainPhoto(int userId, int photoId)
        {
            var user = await _dataContext.GetUser(userId, true);
            Result result = new Result();

            if(!user.Photos.Any(p => p.Id == photoId))
            {
                result.Data = null;
            }

            var photoFromDb = await _dataContext.GetPhoto(photoId);

            if (photoFromDb.IsMain)
            {
                result.Message = "This is already the main photo";
            }

            var currentMainPhoto = await _dataContext.GetMainPhoto(userId);
            currentMainPhoto.IsMain = false;
            photoFromDb.IsMain = true;

            if (await _dataContext.SaveAll())
            {
                result.isValid = true;
            }

            return result;
        }

        public async Task<Result> DeletePhoto(int usedId, int id)
        {
            Result result = new Result();
            var user = await _dataContext.GetUser(usedId, true);
            var photo = await _dataContext.GetPhoto(id);

            if (!user.Photos.Any(p => p.Id == id))
            {
                result.Data = null;
            }

            if (photo.IsMain)
            {
                result.Message = "You cannot delete your main photo";
            }

            if (photo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photo.PublicId);
                var cloudinaryResponse = _cloudinary.Destroy(deleteParams);

                if (cloudinaryResponse.Result == "ok")
                {
                    _dataContext.Delete(photo);
                }
            }

            if (photo.PublicId == null)
            {
                _dataContext.Delete(photo);
            }
            
            if (await _dataContext.SaveAll())
            {
                _result.isValid = true;
            }

            return result;
        }
    }
}
