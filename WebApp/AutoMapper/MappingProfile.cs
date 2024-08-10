using AutoMapper;
using WebApp.DataTransferObjects;
using WebApp.Models;

namespace WebApp.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CourseUploadViewModel, CourseDTO>();
        CreateMap<CourseDTO, CourseUploadViewModel>();
        CreateMap<CourseDTO, CourseViewModel>();

        CreateMap<TagDTO, TagViewModel>();
        CreateMap<TagDTO, CourseTypeViewModel>();
        CreateMap<TagViewModel, TagDTO>();

        CreateMap<CourseTypeDTO, CourseTypeViewModel>();
        CreateMap<CourseTypeViewModel, CourseTypeDTO>();

        CreateMap<RegisterViewModel, RegisterDTO>();
        CreateMap<RegisterDTO, RegisterViewModel>();

        CreateMap<UserDTO, ProfileViewModel>();
        CreateMap<ProfileViewModel, UserDTO>();
    }
}