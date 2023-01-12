using Microsoft.AspNetCore.Mvc;
using Model;
using Services;

namespace Presentation.ViewComponents
{
    public class StudentsTableViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentsTableViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? groupId = null)
        {
            IEnumerable<Student> students;

            if (groupId.HasValue)
            {
                students = await _unitOfWork.StudentsRepository.GetStudentsByGroupIdAsync(groupId.Value);
            }
            else
            {
                students = await _unitOfWork.StudentsRepository.GetAllAsync();
            }

            return View(students);
        }
    }
}
