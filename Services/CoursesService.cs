using Services.Interfaces;

namespace Services
{
    public class CoursesService : ICoursesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoursesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CourseExists(int id)
        {
            if (await _unitOfWork.CoursesRepository.GetByIdAsync(id) == null)
            {
                return false;
            }
            return true;
        }
    }
}
