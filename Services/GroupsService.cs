using Model;
using Infrastructure;

namespace Services
{
    public class GroupsService : IGroupsService
    {
        private readonly IGroupsRepository _groupsRepository;

        public GroupsService(IGroupsRepository groupsRepository)
        {
            _groupsRepository = groupsRepository;
        }

        public async Task<List<Group>> GetAllGroupsAsync()
        {
            return await _groupsRepository.GetAllGroupsAsync();
        }

        public async Task<Group> GetGroupByIdAsync(int id)
        {
            return await _groupsRepository.GetGroupByIdAsync(id);
        }

        public void Insert(Group group)
        {
            _groupsRepository.Insert(group);
        }

        public void Delete(int groupId)
        {
            _groupsRepository.Delete(groupId);
        }

        public void Update(Group group)
        {
            _groupsRepository.Update(group);
        }

        public async Task SaveChangesAsync()
        {
            await _groupsRepository.SaveChangesAsync();
        }

        public async Task<List<Group>> GetGroupsByCourseIdAsync(int courseId)
        {
            return await _groupsRepository.GetGroupsByCourseIdAsync(courseId);
        }
    }
}
