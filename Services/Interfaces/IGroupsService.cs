using Model;

namespace Services
{
    public interface IGroupsService
    {
        public Task<List<Group>> GetAllGroupsAsync();
        public Task<Group> GetGroupByIdAsync(int id);
        public void Insert(Group group);
        public void Delete(int groupId);
        public void Update(Group group);
        public Task SaveChangesAsync();
        public Task<List<Group>> GetGroupsByCourseIdAsync(int courseId);
    }
}
