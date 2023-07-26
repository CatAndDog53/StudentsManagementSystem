using Infrastructure;
using Services.Interfaces;
using Services;
using ViewModels;

namespace ServicesTests
{
    internal class GroupsServiceTests : ServiceTests
    {
        private static readonly int[] _nonExistingIds = { -5, 0, 14, 25, 1015, 4120, 10253 };

        [Test]
        public async Task GetByIdAsync_ExistingIds_ReturnsCorrectGroup()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            for (int i = 1; i < _databaseData.Groups.Length; i++)
            {
                GroupViewModel? expected = _mapper.Map<GroupViewModel>(_databaseData.Groups[i]);
                GroupViewModel? actual = await groupsService.GetByIdAsync(_databaseData.Groups[i].GroupId);

                Assert.That(actual.IsEquivalentTo(expected));
            }
        }

        [Test]
        public async Task GetByIdAsync_NonExistingIds_ReturnsNull()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            for (int i = 0; i < _nonExistingIds.Length; i++)
            {
                GroupViewModel? actual = await groupsService.GetByIdAsync(_nonExistingIds[i]);

                Assert.IsNull(actual);
            }
        }

        [Test]
        public async Task GetAllAsync_ReturnsCorrectGroups()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            IEnumerable<GroupViewModel> expected = _mapper.Map<IEnumerable<GroupViewModel>>(
                    _databaseData.Groups.ToList());
            IEnumerable<GroupViewModel> actual = await groupsService.GetAllAsync();
            actual = actual.OrderBy(group => group.GroupId);

            Assert.That(actual.Count(), Is.EqualTo(expected.Count()));
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.That(actual.ElementAtOrDefault(i).IsEquivalentTo(expected.ElementAtOrDefault(i)));
            }
        }

        [Test]
        public async Task Add_AddsGroup()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            GroupViewModel groupExpected = new GroupViewModel
            {
                GroupId = 225,
                CourseId = 1,
                Course = _mapper.Map<CourseViewModel>(
                    _databaseData.Courses.FirstOrDefault(course => course.CourseId == 1)),
                Name = "Name of the test group"
            };

            await groupsService.Add(groupExpected);
            GroupViewModel groupActual = await groupsService.GetByIdAsync(groupExpected.GroupId);

            Assert.That(groupActual.IsEquivalentTo(groupExpected));
        }

        [Test]
        public async Task Update_UpdatesGroup()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            GroupViewModel groupExpeted = await groupsService.GetByIdAsync(1);
            groupExpeted.CourseId = 1;
            groupExpeted.Course = _mapper.Map<CourseViewModel>(
                    _databaseData.Courses.FirstOrDefault(course => course.CourseId == 1));
            groupExpeted.Name = "Name of the test group";

            await groupsService.Update(groupExpeted);
            GroupViewModel groupActual = await groupsService.GetByIdAsync(groupExpeted.GroupId);

            Assert.That(groupActual.IsEquivalentTo(groupExpeted));
        }

        [Test]
        public async Task Remove_RemovesGroup()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            GroupViewModel group = _mapper.Map<GroupViewModel>(
                _databaseData.Groups.FirstOrDefault(group => group.GroupId == 1));
            await RemoveRelatedStudents(group);
            GroupViewModel groupToRemove = await groupsService.GetByIdAsync(1);
            await groupsService.Remove(group);

            GroupViewModel removedGroup = await groupsService.GetByIdAsync(group.GroupId);
            Assert.IsNull(removedGroup);
        }

        [Test]
        public async Task Remove_GroupWithRelatedStudents_ThrowsException()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            GroupViewModel groupWithRelatedStudents = await groupsService.GetByIdAsync(1);

            Assert.ThrowsAsync<InvalidOperationException>(async () => await groupsService.Remove(groupWithRelatedStudents));
        }

        [Test]
        public async Task GroupExists_ExistingIds_ReturnsTrue()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            for (int i = 0; i < _databaseData.Groups.Count(); i++)
            {
                Assert.That(await groupsService.GroupExists(_databaseData.Groups[i].GroupId), Is.True);
            }
        }

        [Test]
        public async Task GroupExists_NonExistingIds_ReturnsFalse()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            for (int i = 0; i < _nonExistingIds.Length; i++)
            {
                Assert.That(await groupsService.GroupExists(_nonExistingIds[i]), Is.False);
            }
        }

        private async Task RemoveRelatedStudents(GroupViewModel group)
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IStudentsService studentsService = new StudentsService(unitOfWork, _mapper);

            var relatedStudents = await studentsService.GetStudentsByGroupIdAsync(group.GroupId);
            foreach (StudentViewModel student in relatedStudents)
            {
                await studentsService.Remove(student);
            }
        }

    }
}
