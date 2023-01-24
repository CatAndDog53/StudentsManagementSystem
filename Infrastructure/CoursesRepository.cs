using Model;

namespace Infrastructure
{
    public class CoursesRepository : Repository<Course>, ICoursesRepository
    {
        public CoursesRepository(CoursesDbContext dbContext) : base(dbContext) 
        { }
    }
}