using AutoMapper;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Model;
using Presentation.Mappings;
using Services;

namespace ServicesTests
{
    internal abstract class ServiceTests
    {
        protected class DatabaseData
        {
            public Course[] Courses { get; }
            public Group[] Groups { get; }
            public Student[] Students { get; }

            public DatabaseData(Course[] courses, Group[] groups, Student[] students)
            {
                Courses = courses;
                Groups = groups;
                Students = students;
            }
        }

        protected DbContextOptions<CoursesDbContext> _dbContextOptions = new DbContextOptionsBuilder<CoursesDbContext>()
            .UseInMemoryDatabase(databaseName: "CoursesDb")
            .Options;
        protected CoursesDbContext _dbContext;
        protected IUnitOfWork _unitOfWork;
        protected IMapper _mapper;

        protected static DatabaseData _databaseData = new DatabaseData(
            new Course[]
            {
                new Course { CourseId = 1, Name = "C#/.Net", Description = "Basic course to learn C# and .Net platform" },
                new Course { CourseId = 2, Name = "Java SPRING Course", Description = "Basic course to learn Java" },
                new Course { CourseId = 3, Name = "FrontEnd Development (Angular)", Description = "Basic course to learn JavaScript and ANGULAR framework" }
            },
            new Group[]
            {
                new Group { GroupId = 1, CourseId = 2, Name = "Java_Group_1" },
                new Group { GroupId = 3, CourseId = 3, Name = "Angular_Group_1" },
                new Group { GroupId = 5, CourseId = 1, Name = "C#/.Net_Group_1" }
            },
            new Student[]
            {
                new Student { StudentId = 1, GroupId = 1, FirstName = "Haaris", LastName = "Floyd" },
                new Student { StudentId = 2, GroupId = 1, FirstName = "Harmony", LastName = "Chan" },
                new Student { StudentId = 3, GroupId = 3, FirstName = "Owain", LastName = "Armstrong" },
                new Student { StudentId = 4, GroupId = 3, FirstName = "Tyrone", LastName = "Baxter" },
                new Student { StudentId = 5, GroupId = 5, FirstName = "Rahim", LastName = "Hensley" }
            });

        [SetUp]
        public void Setup()
        {
            _dbContext = new CoursesDbContext(_dbContextOptions);
            _unitOfWork = new UnitOfWork(_dbContext);
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }));

            _dbContext.Database.EnsureCreated();
            SeedDatabase();
        }

        [TearDown]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
            _unitOfWork.Dispose();
        }

        private void SeedDatabase()
        {
            _dbContext.Courses.AddRange(_databaseData.Courses);
            _dbContext.Groups.AddRange(_databaseData.Groups);
            _dbContext.Students.AddRange(_databaseData.Students);

            _dbContext.SaveChanges();
        }
    }
}
