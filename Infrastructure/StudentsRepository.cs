using Microsoft.EntityFrameworkCore;
using Model;

namespace Infrastructure
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly CoursesDbContext _dbContext;

        public StudentsRepository(CoursesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _dbContext.Students.SingleOrDefaultAsync(student => student.StudentId == id);
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _dbContext.Students.ToListAsync();
        }

        public void Insert(Student student)
        {
            _dbContext.Students.Add(student);
        }

        public void Delete(int studentId)
        {
            Student student = _dbContext.Students.Find(studentId);
            _dbContext.Students.Remove(student);
        }

        public void Update(Student student)
        {
            _dbContext.Entry(student).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
