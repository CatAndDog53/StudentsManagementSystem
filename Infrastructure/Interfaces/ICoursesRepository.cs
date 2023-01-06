using Model;

namespace Infrastructure
{
    public interface ICoursesRepository
    {
        public Task<Course> GetCourseByIdAsync(int id);
        public Task<List<Course>> GetAllCoursesAsync();
        public void Insert(Course course);
        public void Delete(int courseId);
        public void Update(Course course);
        public Task SaveChangesAsync();
    }
}
