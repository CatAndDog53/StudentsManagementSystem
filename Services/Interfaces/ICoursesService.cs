using ViewModels;

namespace Services.Interfaces
{
    public interface ICoursesService : IViewModelService<CourseViewModel>
    {
        Task<bool> CourseExists(int id);
    }
}
