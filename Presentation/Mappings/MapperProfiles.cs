using AutoMapper;
using Model;
using Presentation.ViewModels;

namespace Presentation.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Course, CourseViewModel>().ReverseMap();
            CreateMap<Group, GroupViewModel>().ReverseMap();
            CreateMap<Student, StudentViewModel>().ReverseMap();
        }
    }
}
