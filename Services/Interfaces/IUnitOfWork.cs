using Infrastructure;

namespace Services
{
    public interface IUnitOfWork : IDisposable
    {
        ICoursesRepository CoursesRepository { get; }
        IGroupsRepository GroupsRepository { get; }
        IStudentsRepository StudentsRepository { get; }

        Task SaveChangesAsync();
    }
}
