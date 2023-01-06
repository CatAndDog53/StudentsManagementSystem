using Microsoft.EntityFrameworkCore;
using Model;

namespace Infrastructure
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly CoursesDbContext _dbContext;

        public CoursesRepository(CoursesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _dbContext.Courses.SingleOrDefaultAsync(course => course.CourseId == id);
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _dbContext.Courses.ToListAsync();
        }

        public void Insert(Course course)
        {
            _dbContext.Courses.Add(course);
        }

        public void Delete(int courseId)
        {
            Course course = _dbContext.Courses.Find(courseId);
            _dbContext.Remove(course);
        }

        public void Update(Course course)
        {
            _dbContext.Entry(course).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}