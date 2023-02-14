using Model;
using Moq;
using Services;

namespace ServicesTests
{
    internal class CoursesServiceTests
    {
        private static readonly object[] _testCasesForCourseExistsCorrectInputTest =
        {
            new object[] { 0, new Course(), true },
            new object[] { 1, null, false }
        };

        [TestCaseSource(nameof(_testCasesForCourseExistsCorrectInputTest))]
        public void CourseExists_CorrectInputTest_ShouldReturnCorrectResult(int courseId, Course course, bool expectedResult)
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            unitOfWorkStub.Setup(x => x.CoursesRepository.GetByIdAsync(courseId).Result)
                              .Returns(course);
            var actualResult = new CoursesService(unitOfWorkStub.Object).CourseExists(courseId).Result;

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}