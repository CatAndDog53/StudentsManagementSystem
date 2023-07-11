using ViewModels;
using Services;
using Services.Interfaces;
using Infrastructure;

namespace ServicesTests
{
    internal class CoursesServiceTests : ServiceTests
    {
        private static readonly int[] _nonExistingIds = { -5, 0, 14, 25, 1015, 4120, 10253 };

        [Test]
        public async Task GetByIdAsync_ExistingIds_ReturnsCorrectCourse()
        {
            ICoursesService coursesService = new CoursesService(_unitOfWork, _mapper);

            for (int i = 1; i < _databaseData.Courses.Length; i++)
            {
                CourseViewModel? expected = _mapper.Map<CourseViewModel>(
                    _databaseData.Courses.FirstOrDefault(course => course.CourseId == i));
                CourseViewModel? actual = await coursesService.GetByIdAsync(i);

                Assert.That(actual.IsEquivalentTo(expected));
            }
        }

        [Test]
        public async Task GetByIdAsync_NonExistingIds_ReturnsNull()
        {
            ICoursesService coursesService = new CoursesService(_unitOfWork, _mapper);

            for (int i = 0; i < _nonExistingIds.Length; i++)
            {
                CourseViewModel? actual = await coursesService.GetByIdAsync(_nonExistingIds[i]);

                Assert.IsNull(actual);
            }
        }

        [Test]
        public async Task GetAllAsync_ReturnsCorrectCourses()
        {
            ICoursesService coursesService = new CoursesService(_unitOfWork, _mapper);

            IEnumerable<CourseViewModel> expected = _mapper.Map<IEnumerable<CourseViewModel>>(
                    _databaseData.Courses.ToList());
            IEnumerable<CourseViewModel> actual = await coursesService.GetAllAsync();

            Assert.That(actual.Count(), Is.EqualTo(expected.Count()));
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.That(actual.ElementAtOrDefault(i).IsEquivalentTo(expected.ElementAtOrDefault(i)));
            }
        }

        [Test]
        public async Task Add_AddsCourse()
        {
            ICoursesService coursesService = new CoursesService(_unitOfWork, _mapper);
            CourseViewModel courseExpeted = new CourseViewModel { 
                CourseId = 225, 
                Name = "Name of the test course", 
                Description = "Description of the test course" };
            
            await coursesService.Add(courseExpeted);
            CourseViewModel courseActual = await coursesService.GetByIdAsync(courseExpeted.CourseId);

            Assert.That(courseActual.IsEquivalentTo(courseExpeted));
        }

        [Test]
        public async Task Update_UpdatesCourse()
        {
            ICoursesService coursesService = new CoursesService(_unitOfWork, _mapper);
            CourseViewModel courseExpeted = await coursesService.GetByIdAsync(1);

            courseExpeted.Name = "Test";
            courseExpeted.Description = "Test";
            await coursesService.Update(courseExpeted);

            CourseViewModel courseActual = await coursesService.GetByIdAsync(courseExpeted.CourseId);
            Assert.That(courseActual.IsEquivalentTo(courseExpeted));
        }

        [Test]
        public async Task Remove_RemovesCourse()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            ICoursesService coursesService = new CoursesService(unitOfWork, _mapper);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);
            IStudentsService studentsService = new StudentsService(unitOfWork, _mapper);

            CourseViewModel course = _mapper.Map<CourseViewModel>(
                _databaseData.Courses.FirstOrDefault(course => course.CourseId == 1));
            await RemoveRelatedGroupsAndStudents(course);

            await coursesService.Remove(course);

            CourseViewModel removedCourse = await coursesService.GetByIdAsync(course.CourseId);
            Assert.IsNull(removedCourse);
        }

        [Test]
        public void Remove_CourseWithRelatedGroups_ThrowsException()
        {
            ICoursesService coursesService = new CoursesService(_unitOfWork, _mapper);
            CourseViewModel courseWithRelatedGroups = _mapper.Map<CourseViewModel>(
                _databaseData.Courses.FirstOrDefault(course => course.CourseId == 1));

           Assert.ThrowsAsync<InvalidOperationException>(async () => await coursesService.Remove(courseWithRelatedGroups));
        }

        [Test]
        public async Task CourseExists_ExistingIds_ReturnsTrue()
        {
            ICoursesService coursesService = new CoursesService(_unitOfWork, _mapper);

            for(int i = 0; i < _databaseData.Courses.Count(); i++)
            {
                Assert.That(await coursesService.CourseExists(_databaseData.Courses[i].CourseId), Is.True);
            }
        }

        [Test]
        public async Task CourseExists_NonExistingIds_ReturnsFalse()
        {
            ICoursesService coursesService = new CoursesService(_unitOfWork, _mapper);

            for (int i = 0; i < _nonExistingIds.Length; i++)
            {
                Assert.That(await coursesService.CourseExists(_nonExistingIds[i]), Is.False);
            }
        }

        private async Task RemoveRelatedGroupsAndStudents(CourseViewModel course)
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            ICoursesService coursesService = new CoursesService(unitOfWork, _mapper);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);
            IStudentsService studentsService = new StudentsService(unitOfWork, _mapper);

            var relatedGroups = await groupsService.GetGroupsByCourseIdAsync(course.CourseId);
            foreach (GroupViewModel group in relatedGroups)
            {
                var relatedStudents = await studentsService.GetStudentsByGroupIdAsync(group.GroupId);
                foreach (StudentViewModel student in relatedStudents)
                {
                    await studentsService.Remove(student);
                }
                //GroupViewModel groupToRemove = await groupsService.GetByIdAsync(group.GroupId);
                await groupsService.Remove(group);
            }
        }
    }
}