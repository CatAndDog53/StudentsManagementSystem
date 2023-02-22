using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model;
using ViewModels;
using Services;

namespace Presentation.ViewComponents
{
    public class GroupsTableViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GroupsTableViewComponent(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? courseId = null)
        {
            IEnumerable<Group> groups;

            if (courseId.HasValue)
            {
                groups = await _unitOfWork.GroupsRepository.GetGroupsByCourseIdAsync(courseId.Value);
            }
            else
            {
                groups = await _unitOfWork.GroupsRepository.GetAllAsync();
            }  

            var groupsViewModel = _mapper.Map<IEnumerable<GroupViewModel>>(groups);
            return View(groupsViewModel);
        }
    }
}
