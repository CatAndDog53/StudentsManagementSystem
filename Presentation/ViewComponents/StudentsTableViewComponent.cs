using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model;
using Presentation.ViewModels;
using Services;

namespace Presentation.ViewComponents
{
    public class StudentsTableViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentsTableViewComponent(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

            var studentsViewModel = _mapper.Map<IEnumerable<StudentViewModel>>(students);
            return View(studentsViewModel);
        }
    }
}
