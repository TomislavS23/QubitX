using AutoMapper;
using WebAPI.DataTransferObjects;
using WebApp.Models;

namespace WebApp.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CourseUploadViewModel, CourseDTO>();
        CreateMap<CourseDTO, CourseUploadViewModel>();
        CreateMap<CourseDTO, CourseViewModel>();
    }
}