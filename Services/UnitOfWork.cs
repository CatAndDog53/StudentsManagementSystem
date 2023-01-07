using Infrastructure;

namespace Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoursesDbContext _dbContext;
        private ICoursesRepository _coursesRepository;
        private IGroupsRepository _groupsRepository;
        private IStudentsRepository _studentsRepository;

        public ICoursesRepository CoursesRepository => _coursesRepository;
        public IGroupsRepository GroupsRepository => _groupsRepository;
        public IStudentsRepository StudentsRepository => _studentsRepository;

        public UnitOfWork(CoursesDbContext dbContext)
        {
            _dbContext = dbContext;
            _coursesRepository = new CoursesRepository(dbContext);
            _groupsRepository = new GroupsRepository(dbContext);
            _studentsRepository = new StudentsRepository(dbContext);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
