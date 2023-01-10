using Model;

namespace Infrastructure
{
    public interface IStudentsRepository : IRepository<Student>
    {
        public Task<List<Student>> GetStudentsByGroupIdAsync(int groupId);
    }
}
