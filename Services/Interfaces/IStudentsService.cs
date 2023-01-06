using Model;

namespace Services.Interfaces
{
    public interface IStudentsService
    {
        public Task<Student> GetStudentByIdAsync(int id);
        public Task<List<Student>> GetAllStudentsAsync();
        public void Insert(Student student);
        public void Delete(int studentId);
        public void Update(Student student);
        public Task SaveChangesAsync();
    }
}
