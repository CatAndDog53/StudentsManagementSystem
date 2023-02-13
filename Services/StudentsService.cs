using Services.Interfaces;

namespace Services
{
    public class StudentsService : IStudentsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> StudentExists(int id)
        {
            if (await _unitOfWork.StudentsRepository.GetByIdAsync(id) == null)
            {
                return false;
            }
            return true;
        }
    }
}
