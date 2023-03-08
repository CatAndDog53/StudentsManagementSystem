using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model;
using ViewModels;
using Services.Interfaces;

namespace Presentation.ViewComponents
{
    public class StudentsTableViewComponent : ViewComponent
    {
        private readonly IStudentsService _studentsService;

        public StudentsTableViewComponent(IStudentsService studentsService)
        {
            _studentsService = studentsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? groupId = null)
        {
            IEnumerable<StudentViewModel> students;

            if (groupId.HasValue)
            {
                students = await _studentsService.GetStudentsByGroupIdAsync(groupId.Value);
            }
            else
            {
                students = await _studentsService.GetAllAsync();
            }

            return View(students);
        }
    }
}
