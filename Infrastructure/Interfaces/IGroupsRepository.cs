using Model;

namespace Infrastructure
{
    public interface IGroupsRepository : IRepository<Group>
    {
        public Task<IEnumerable<Group>> GetGroupsByCourseIdAsync(int? courseId);
    }
}
