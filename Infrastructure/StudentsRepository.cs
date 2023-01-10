using Microsoft.EntityFrameworkCore;
using Model;

namespace Infrastructure
{
    public class StudentsRepository : Repository<Student>, IStudentsRepository
    {
        public StudentsRepository(CoursesDbContext dbContext) : base(dbContext) 
        { }

        public async Task<List<Student>> GetStudentsByGroupIdAsync(int groupId)
        {
            return await CoursesDbContext.Students.Where(student => student.GroupId == groupId).ToListAsync();
        }
    }
}
