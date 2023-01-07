using Microsoft.EntityFrameworkCore;
using Model;

namespace Infrastructure
{
    public class GroupsRepository : Repository<Group>, IGroupsRepository
    {
        public GroupsRepository(CoursesDbContext context) : base(context) 
        { }

        public async Task<List<Group>> GetGroupsByCourseIdAsync(int courseId)
        {
            return await CoursesDbContext.Groups.Where(group => group.CourseId == courseId).ToListAsync();
        }
    }
}
