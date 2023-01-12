using Microsoft.AspNetCore.Mvc;
using Model;
using Services;

namespace Presentation.ViewComponents
{
    public class GroupsTableViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public GroupsTableViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            return View(groups);
        }
    }
}
