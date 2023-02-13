using Services.Interfaces;

namespace Services
{
    public class GroupsService : IGroupsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GroupsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> GroupExists(int id)
        {
            if (await _unitOfWork.GroupsRepository.GetByIdAsync(id) == null)
            {
                return false;
            }
            return true;
        }
    }
}
