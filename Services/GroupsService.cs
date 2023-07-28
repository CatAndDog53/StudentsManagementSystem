using AutoMapper;
using Model;
using Services.Interfaces;
using ViewModels;

namespace Services
{
    public class GroupsService : IGroupsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GroupsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GroupViewModel> GetByIdAsync(int? id)
        {
            var group = await _unitOfWork.GroupsRepository.GetByIdAsync(id);
            return _mapper.Map<GroupViewModel>(group);
        }

        public async Task<GroupViewModelForUpdate> GetByIdForUpdateAsync(int? id)
        {
            var group = await _unitOfWork.GroupsRepository.GetByIdAsync(id);
            return _mapper.Map<GroupViewModelForUpdate>(group);
        }

        public async Task<IEnumerable<GroupViewModel>> GetAllAsync()
        {
            var groups = await _unitOfWork.GroupsRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GroupViewModel>>(groups);
        }

        public async Task Add(GroupViewModel groupViewModel)
        {
            var group = _mapper.Map<Group>(groupViewModel);
            _unitOfWork.GroupsRepository.Add(group);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(GroupViewModel groupViewModel)
        {
            var group = _mapper.Map<Group>(groupViewModel);
            _unitOfWork.GroupsRepository.Update(group);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(GroupViewModelForUpdate groupViewModel)
        {
            var group = _mapper.Map<Group>(groupViewModel);
            _unitOfWork.GroupsRepository.Update(group);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Remove(GroupViewModel groupViewModel)
        {
            var group = _mapper.Map<Group>(groupViewModel);
            _unitOfWork.GroupsRepository.Remove(group);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ICollection<GroupViewModel>> GetGroupsByCourseIdAsync(int? courseId)
        {
            var groups = await _unitOfWork.GroupsRepository.GetGroupsByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<GroupViewModel>>(groups).ToList();
        }

        public async Task<bool> GroupExists(int id)
        {
            if (await _unitOfWork.GroupsRepository.GetByIdAsync(id) == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsNameUnique(string name)
        {
            var groups = await _unitOfWork.GroupsRepository.GetAllAsync();
            return !groups.Any(group => group.Name == name);
        }

        public async Task<bool> GroupWithSuchNameAndIdExists(string name, int id)
        {
            var groups = await _unitOfWork.GroupsRepository.GetAllAsync();
            return groups.Any(group => group.Name == name && group.GroupId == id);
        }
    }
}
