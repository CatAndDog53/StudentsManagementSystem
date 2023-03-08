using Microsoft.AspNetCore.Mvc;
using ViewModels;
using Services.Interfaces;

namespace Presentation.ViewComponents
{
    public class GroupsTableViewComponent : ViewComponent
    {
        private readonly IGroupsService _groupsService;

        public GroupsTableViewComponent(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? courseId = null)
        {
            IEnumerable<GroupViewModel> groups;

            if (courseId.HasValue)
            {
                groups = await _groupsService.GetGroupsByCourseIdAsync(courseId.Value);
            }
            else
            {
                groups = await _groupsService.GetAllAsync();
            }  

            return View(groups);
        }
    }
}
