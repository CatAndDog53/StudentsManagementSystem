using Model;
using Moq;
using Services;

namespace ServicesTests
{
    internal class GroupsServiceTests
    {
        private static readonly object[] _testCasesForGroupExistsCorrectInputTest =
        {
            new object[] { 0, new Group(), true },
            new object[] { 1, null, false }
        };

        [TestCaseSource(nameof(_testCasesForGroupExistsCorrectInputTest))]
        public void GroupExists_CorrectInputTest_ShouldReturnCorrectResult(int groupId, Group group, bool expectedResult)
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            unitOfWorkStub.Setup(x => x.GroupsRepository.GetByIdAsync(groupId).Result)
                              .Returns(group);
            var actualResult = new GroupsService(unitOfWorkStub.Object).GroupExists(groupId).Result;

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}
