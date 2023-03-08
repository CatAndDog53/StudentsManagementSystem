using ViewModels;

namespace Services.Interfaces
{
    public interface IGroupsService : IViewModelService<GroupViewModel>
    {
        Task<ICollection<GroupViewModel>> GetGroupsByCourseIdAsync(int? id);
        Task<bool> GroupExists(int id);
    }
}
