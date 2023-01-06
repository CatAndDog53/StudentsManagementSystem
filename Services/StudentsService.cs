using Model;
using Infrastructure;
using Services.Interfaces;

namespace Services
{
    public class StudentsService : IStudentsService
    {
        private readonly IStudentsRepository _studentsRepository;

        public StudentsService(IStudentsRepository studentsRepository) 
        {
            _studentsRepository= studentsRepository;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _studentsRepository.GetAllStudentsAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _studentsRepository.GetStudentByIdAsync(id);
        }

        public void Insert(Student student)
        {
            _studentsRepository.Insert(student);
        }

        public void Delete(int studentId)
        {
            _studentsRepository.Delete(studentId);
        }

        public void Update(Student student)
        {
            _studentsRepository.Update(student);
        }

        public async Task SaveChangesAsync()
        {
            await _studentsRepository.SaveChangesAsync();
        }
    }
}
