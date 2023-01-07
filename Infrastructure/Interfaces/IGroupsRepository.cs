using Model;

namespace Infrastructure
{
    public interface IGroupsRepository : IRepository<Group>
    {
        public Task<List<Group>> GetGroupsByCourseIdAsync(int courseId);
    }
}
