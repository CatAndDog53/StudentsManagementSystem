using Model;
using Infrastructure;

namespace Services
{
    public class CoursesService : ICoursesService
    {
        private readonly ICoursesRepository _coursesRepository;

        public CoursesService(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        public async Task<List<Course>> GetAllCoursesAsync() 
        {
            return await _coursesRepository.GetAllCoursesAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _coursesRepository.GetCourseByIdAsync(id);
        }

        public void Insert(Course course) 
        {
            _coursesRepository.Insert(course);
        }

        public void Update(Course course) 
        { 
            _coursesRepository.Update(course);
        }

        public void Delete(int id)
        {
            _coursesRepository.Delete(id);
        }

        public async Task SaveChangesAsync()
        {
            await _coursesRepository.SaveChangesAsync();
        }


    }
}