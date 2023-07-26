using Infrastructure;
using Services.Interfaces;
using Services;
using ViewModels;

namespace ServicesTests
{
    internal class StudentsServiceTests : ServiceTests
    {
        private static readonly int[] _nonExistingIds = { -5, 0, 14, 25, 1015, 4120, 10253 };

        [Test]
        public async Task GetByIdAsync_ExistingIds_ReturnsCorrectStudent()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IStudentsService studentsService = new StudentsService(unitOfWork, _mapper);

            for (int i = 1; i < _databaseData.Students.Length; i++)
            {
                StudentViewModel? expected = _mapper.Map<StudentViewModel>(_databaseData.Students[i]);
                StudentViewModel? actual = await studentsService.GetByIdAsync(_databaseData.Students[i].StudentId);

                Assert.That(actual.IsEquivalentTo(expected));
            }
        }

        [Test]
        public async Task GetByIdAsync_NonExistingIds_ReturnsNull()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IStudentsService studentsService = new StudentsService(unitOfWork, _mapper);

            for (int i = 0; i < _nonExistingIds.Length; i++)
            {
                StudentViewModel? actual = await studentsService.GetByIdAsync(_nonExistingIds[i]);

                Assert.IsNull(actual);
            }
        }

        [Test]
        public async Task GetAllAsync_ReturnsCorrectStudents()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IStudentsService studentsService = new StudentsService(unitOfWork, _mapper);

            IEnumerable<StudentViewModel> expected = _mapper.Map<IEnumerable<StudentViewModel>>(
                    _databaseData.Students.ToList());
            IEnumerable<StudentViewModel> actual = await studentsService.GetAllAsync();
            actual = actual.OrderBy(student => student.StudentId);

            Assert.That(actual.Count(), Is.EqualTo(expected.Count()));
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.That(actual.ElementAtOrDefault(i).IsEquivalentTo(expected.ElementAtOrDefault(i)));
            }
        }

        [Test]
        public async Task Add_AddsStudent()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IStudentsService studentsService = new StudentsService(unitOfWork, _mapper);

            StudentViewModel studentExpected = new StudentViewModel
            {
                StudentId = 225,
                GroupId = 3,
                Group = _mapper.Map<GroupViewModel>(
                    _databaseData.Groups.FirstOrDefault(group => group.GroupId == 3)),
                FirstName = "First name of the test student",
                LastName = "Last name of the test student"
            };

            await studentsService.Add(studentExpected);
            StudentViewModel studentActual = await studentsService.GetByIdAsync(studentExpected.StudentId);

            Assert.That(studentActual.IsEquivalentTo(studentExpected));
        }

        [Test]
        public async Task Update_UpdatesStudent()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IStudentsService studentsService = new StudentsService(unitOfWork, _mapper);

            StudentViewModel studentExpeted = await studentsService.GetByIdAsync(1);
            studentExpeted.GroupId = 3;
            studentExpeted.Group = _mapper.Map<GroupViewModel>(
                    _databaseData.Groups.FirstOrDefault(group => group.GroupId == 3));
            studentExpeted.FirstName = "First name of the test student";
            studentExpeted.LastName = "Last name of the test student";

            await studentsService.Update(studentExpeted);
            StudentViewModel studentActual = await studentsService.GetByIdAsync(studentExpeted.StudentId);

            Assert.That(studentActual.IsEquivalentTo(studentExpeted));
        }

        [Test]
        public async Task Remove_RemovesStudent()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IStudentsService studentsService = new StudentsService(unitOfWork, _mapper);

            StudentViewModel studentToRemove = await studentsService.GetByIdAsync(1);
            await studentsService.Remove(studentToRemove);

            StudentViewModel removedStudent = await studentsService.GetByIdAsync(studentToRemove.GroupId);
            Assert.IsNull(removedStudent);
        }

        [Test]
        public async Task StudentExists_ExistingIds_ReturnsTrue()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IStudentsService studentsService = new StudentsService(unitOfWork, _mapper);

            for (int i = 0; i < _databaseData.Students.Count(); i++)
            {
                Assert.That(await studentsService.StudentExists(_databaseData.Students[i].StudentId), Is.True);
            }
        }

        [Test]
        public async Task StudentExists_NonExistingIds_ReturnsFalse()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IStudentsService studentsService = new StudentsService(unitOfWork, _mapper);

            for (int i = 0; i < _nonExistingIds.Length; i++)
            {
                Assert.That(await studentsService.StudentExists(_nonExistingIds[i]), Is.False);
            }
        }
    }
}
