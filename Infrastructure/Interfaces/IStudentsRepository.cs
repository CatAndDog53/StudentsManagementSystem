using Model;

namespace Infrastructure
{
    public interface IStudentsRepository : IRepository<Student>
    {
        public Task<IEnumerable<Student>> GetStudentsByGroupIdAsync(int? groupId);
    }
}
