using Model;

namespace Services
{
    public interface ICoursesService
    {
        Task<Course> GetCourseByIdAsync(int id);
        Task<List<Course>> GetAllCoursesAsync();
        void Insert(Course course);
        void Delete(int courseId);
        void Update(Course course);
        Task SaveChangesAsync();
    }
}
