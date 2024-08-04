using AutoMapper;
using WebAPI.DataTransferObjects;
using WebAPI.Models;

namespace WebAPI.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDTO>();
        
        // Tag mappings
        CreateMap<Tag, TagDTO>();
        
        // CourseTypes mappings
        CreateMap<CourseType, CourseTypeDTO>();
        
        // Course mappings
        CreateMap<Course, CourseDTO>();
        CreateMap<CourseDTO, Course>();
        CreateMap<CreateCourseDTO, Course>();
    }
}