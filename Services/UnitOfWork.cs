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

        public UnitOfWork(CoursesDbContext dbContext, ICoursesRepository coursesRepository, IGroupsRepository groupsRepository, 
            IStudentsRepository studentsRepository)
        {
            _dbContext = dbContext;
            _coursesRepository = coursesRepository;
            _groupsRepository = groupsRepository;
            _studentsRepository = studentsRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
