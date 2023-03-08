using ViewModels;

namespace Services.Interfaces
{
    public interface IStudentsService : IViewModelService<StudentViewModel>
    {
        Task<ICollection<StudentViewModel>> GetStudentsByGroupIdAsync(int? groupId);
        Task<bool> StudentExists(int id);
    }
}
