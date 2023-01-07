using Microsoft.EntityFrameworkCore;
using Model;

namespace Infrastructure
{
    public class StudentsRepository : Repository<Student>, IStudentsRepository
    {
        public StudentsRepository(CoursesDbContext dbContext) : base(dbContext) 
        { }
    }
}
