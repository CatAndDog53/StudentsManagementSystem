using ViewModels;

namespace Services.Interfaces
{
    public interface IGroupsService : IViewModelService<GroupViewModel>
    {
        Task<GroupViewModelForUpdate> GetByIdForUpdateAsync(int? id);
        Task Update(GroupViewModelForUpdate groupViewModel);
        Task<ICollection<GroupViewModel>> GetGroupsByCourseIdAsync(int? id);
        Task<bool> GroupExists(int id);
        Task<bool> IsNameUnique(string name);
        Task<bool> GroupWithSuchNameAndIdExists(string name, int id);
    }
}
