using Microsoft.EntityFrameworkCore;
using Model;

namespace Infrastructure
{
    public class GroupsRepository : IGroupsRepository
    {
        private readonly CoursesDbContext _dbContext;

        public GroupsRepository(CoursesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Group> GetGroupByIdAsync(int id)
        {
            return await _dbContext.Groups.SingleOrDefaultAsync(group => group.GroupId == id);
        }

        public async Task<List<Group>> GetAllGroupsAsync()
        {
            return await _dbContext.Groups.ToListAsync();
        }

        public void Insert(Group group)
        {
            _dbContext.Groups.Add(group);
        }

        public void Delete(int groupId)
        {
            Group group = _dbContext.Groups.Find(groupId);
            _dbContext.Groups.Remove(group);
        }

        public void Update(Group group)
        {
            _dbContext.Entry(group).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Group>> GetGroupsByCourseIdAsync(int courseId)
        {
            return await _dbContext.Groups.Where(group => group.CourseId == courseId).ToListAsync();
        }
    }
}
