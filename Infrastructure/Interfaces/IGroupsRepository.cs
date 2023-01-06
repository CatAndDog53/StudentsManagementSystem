using Model;

namespace Infrastructure
{
    public interface IGroupsRepository
    {
        public Task<Group> GetGroupByIdAsync(int id);
        public Task<List<Group>> GetAllGroupsAsync();
        public void Insert(Group group);
        public void Delete(int groupId);
        public void Update(Group group);
        public Task SaveChangesAsync();
        public Task<List<Group>> GetGroupsByCourseIdAsync(int courseId);
    }
}
