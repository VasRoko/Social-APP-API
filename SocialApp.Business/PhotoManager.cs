using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using SocialApp.Business.Interface;
using SocialApp.DataAccess.Interfaces;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;

namespace SocialApp.Business
{
    public class PhotoManager : IPhotoManager
    {
        private readonly ISocialAppDataAccess _dataContext;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly IMapper _mapper;
        private Cloudinary _cloudinary;

        public PhotoManager(ISocialAppDataAccess dataContext, IOptions<CloudinarySettings> cloudinaryConfig, IMapper mapper)
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
        }

        public async Task<PhotoForReturnDto> AddPhotoUser(int userId, PhotoForCreationDto photoForCreationDto)
        {

            var user = await _dataContext.GetUser(userId);
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
    }
}
