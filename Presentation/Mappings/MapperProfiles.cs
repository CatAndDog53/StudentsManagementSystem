using AutoMapper;
using Model;
using ViewModels;

namespace Presentation.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Course, CourseViewModel>().ReverseMap();
            CreateMap<Group, GroupViewModel>().ReverseMap();
            CreateMap<Group, GroupViewModelForUpdate>().ReverseMap();
            CreateMap<GroupViewModel, GroupViewModelForUpdate>().ReverseMap();
            CreateMap<Student, StudentViewModel>().ReverseMap();
        }
    }
}
