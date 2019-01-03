using System.Linq;
using AutoMapper;
using SocialApp.Domain;
using SocialApp.Domain.Dtos;
using SocialApp.Models;

namespace SocialApp.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>().ForMember(dest => dest.PhotoUrl,
                opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); }).ForMember(
                dest => dest.Age, opt => { opt.ResolveUsing(d => d.DateOfBirth.CalculateAge()); });
            CreateMap<User, UserForDetailedDto>().ForMember(dest => dest.PhotoUrl,
                opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); }).ForMember(
                dest => dest.Age, opt => { opt.ResolveUsing(d => d.DateOfBirth.CalculateAge()); });
            CreateMap<UserForRegisterDto, UserRegister>();
            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
        }
    }
}
