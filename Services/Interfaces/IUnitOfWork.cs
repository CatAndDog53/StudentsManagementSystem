using Infrastructure;

namespace Services
{
    public interface IUnitOfWork
    {
        public ICoursesRepository CoursesRepository { get; }
        public IGroupsRepository GroupsRepository { get; }
        public IStudentsRepository StudentsRepository { get; }

        public Task SaveChangesAsync();
    }
}
