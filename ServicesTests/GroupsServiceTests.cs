using Infrastructure;
using Services.Interfaces;
using Services;
using ViewModels;

namespace ServicesTests
{
    internal class GroupsServiceTests : ServiceTests
    {
        private class GroupWithSuchNameAndIdExistsTestCase
        {
            public string Name { get; private set; }
            public int Id { get; private set; }
            public bool ExpectedResult { get; private set; }

            public GroupWithSuchNameAndIdExistsTestCase(string name, int id, bool expectedResult)
            {
                Name = name;
                Id = id; 
                ExpectedResult = expectedResult;
            }
        }

        private static readonly int[] _nonExistingIds = { -5, 0, 14, 25, 1015, 4120, 10253 };
        private static readonly string[] _nonExistingNames = { "Non-existing name 1", "Non-existing name 2" };
        private static readonly GroupWithSuchNameAndIdExistsTestCase[] _groupWithSuchNameAndIdExistsTestCases =
        {
            new GroupWithSuchNameAndIdExistsTestCase(_databaseData.Groups[0].Name, _databaseData.Groups[0].GroupId, true),
            new GroupWithSuchNameAndIdExistsTestCase(_databaseData.Groups[2].Name, _databaseData.Groups[2].GroupId, true),
            new GroupWithSuchNameAndIdExistsTestCase(_databaseData.Groups[0].Name, 404, false),
            new GroupWithSuchNameAndIdExistsTestCase("Non-existing name", _databaseData.Groups[0].GroupId, false),
            new GroupWithSuchNameAndIdExistsTestCase("Non-existing name", 404, false)
        };

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
        public async Task GetByIdForUpdateAsync_ExistingIds_ReturnsCorrectGroup()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            for (int i = 1; i < _databaseData.Groups.Length; i++)
            {
                GroupViewModelForUpdate? expected = _mapper.Map<GroupViewModelForUpdate>(_databaseData.Groups[i]);
                GroupViewModelForUpdate? actual = await groupsService.GetByIdForUpdateAsync(_databaseData.Groups[i].GroupId);

                Assert.That(actual.IsEquivalentTo(expected));
            }
        }

        [Test]
        public async Task GetByIdForUpdateAsync_NonExistingIds_ReturnsNull()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            for (int i = 0; i < _nonExistingIds.Length; i++)
            {
                GroupViewModelForUpdate? actual = await groupsService.GetByIdForUpdateAsync(_nonExistingIds[i]);

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
        public async Task Update_GroupViewModelInput_UpdatesGroup()
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
        public async Task Update_GroupViewModelForUpdateInput_UpdatesGroup()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            GroupViewModel groupExpeted = await groupsService.GetByIdAsync(1);
            groupExpeted.CourseId = 1;
            groupExpeted.Course = _mapper.Map<CourseViewModel>(
                    _databaseData.Courses.FirstOrDefault(course => course.CourseId == 1));
            groupExpeted.Name = "Name of the test group";

            await groupsService.Update(_mapper.Map<GroupViewModelForUpdate>(groupExpeted));
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

        [Test]
        public async Task IsNameUnique_NonExistingNames_ReturnsTrue()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            for (int i = 0; i < _nonExistingNames.Length; i++)
            {
                Assert.That(await groupsService.IsNameUnique(_nonExistingNames[i]), Is.True);
            }
        }

        [Test]
        public async Task IsNameUnique_ExistingNames_ReturnsFalse()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            for (int i = 0; i < _databaseData.Groups.Length; i++)
            {
                Assert.That(await groupsService.IsNameUnique(_databaseData.Groups[i].Name), Is.False);
            }
        }

        [Test]
        public async Task GroupWithSuchNameAndIdExists_ExistingGroups_ReturnsTrue()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            for (int i = 0; i < _databaseData.Groups.Length; i++)
            {
                var group = _databaseData.Groups[i];
                Assert.That(await groupsService.GroupWithSuchNameAndIdExists(group.Name, group.GroupId), Is.True);
            }
        }

        [Test]
        public async Task GroupWithSuchNameAndIdExists_NonExistingGroups_ReturnsFalse()
        {
            CoursesDbContext dbContext = new CoursesDbContext(_dbContextOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IGroupsService groupsService = new GroupsService(unitOfWork, _mapper);

            for (int i = 0; i < _groupWithSuchNameAndIdExistsTestCases.Length; i++)
            {
                var testCase = _groupWithSuchNameAndIdExistsTestCases[i];
                bool actualResult = await groupsService.GroupWithSuchNameAndIdExists(testCase.Name, testCase.Id);
                Assert.That(actualResult, Is.EqualTo(testCase.ExpectedResult));
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
