using Model;
using Moq;
using Services;

namespace ServicesTests
{
    internal class StudentsServiceTests
    {
        private static readonly object[] _testCasesForStudentExistsCorrectInputTest =
        {
            new object[] { 0, new Student(), true },
            new object[] { 1, null, false }
        };

        [TestCaseSource(nameof(_testCasesForStudentExistsCorrectInputTest))]
        public void StudentExists_CorrectInputTest_ShouldReturnCorrectResult(int studentId, Student student, bool expectedResult)
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            unitOfWorkStub.Setup(x => x.StudentsRepository.GetByIdAsync(studentId).Result)
                              .Returns(student);
            var actualResult = new StudentsService(unitOfWorkStub.Object).StudentExists(studentId).Result;

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}
