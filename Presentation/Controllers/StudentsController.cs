using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using Presentation.ViewModels;
using Services;
using Services.Interfaces;

namespace Presentation.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentsService _studentsService;
        private readonly IMapper _mapper;

        public StudentsController(IUnitOfWork unitOfWork, IStudentsService studentsService, IMapper mapper)
        {
            _unitOfWork= unitOfWork;
            _studentsService= studentsService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var studentsViewModel = _mapper.Map<IEnumerable<StudentViewModel>>(await _unitOfWork.StudentsRepository.GetAllAsync());
            return View(studentsViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _unitOfWork.StudentsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var student = await _unitOfWork.StudentsRepository.GetByIdAsync(id.GetValueOrDefault());
            if (student == null)
            {
                return NotFound();
            }

            var studentViewModel = _mapper.Map<StudentViewModel>(student);
            return View(studentViewModel);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Groups = new SelectList(
                _mapper.Map<IEnumerable<GroupViewModel>>(await _unitOfWork.GroupsRepository.GetAllAsync()), 
                "GroupId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,GroupId,FirstName,LastName")] StudentViewModel studentViewModel)
        {
            ViewBag.Groups = new SelectList(
                _mapper.Map<IEnumerable<GroupViewModel>>(await _unitOfWork.GroupsRepository.GetAllAsync()),
                "GroupId", "Name");

            if (ModelState.IsValid)
            {
                _unitOfWork.StudentsRepository.Add(_mapper.Map<Student>(studentViewModel));
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studentViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Groups = new SelectList(
                _mapper.Map<IEnumerable<GroupViewModel>>(await _unitOfWork.GroupsRepository.GetAllAsync()),
                "GroupId", "Name");

            if (id == null || await _unitOfWork.StudentsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var student = await _unitOfWork.StudentsRepository.GetByIdAsync(id.GetValueOrDefault());
            if (student == null)
            {
                return NotFound();
            }

            var studentViewModel = _mapper.Map<StudentViewModel>(student);
            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,GroupId,FirstName,LastName")] StudentViewModel studentViewModel)
        {
            ViewBag.Groups = new SelectList(
                _mapper.Map<IEnumerable<GroupViewModel>>(await _unitOfWork.GroupsRepository.GetAllAsync()),
                "GroupId", "Name");

            if (id != studentViewModel.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var student = _mapper.Map<Student>(studentViewModel);
                try
                {
                    _unitOfWork.StudentsRepository.Update(student);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _studentsService.StudentExists(student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(studentViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _unitOfWork.StudentsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var student = await _unitOfWork.StudentsRepository.GetByIdAsync(id.GetValueOrDefault());
            if (student == null)
            {
                return NotFound();
            }

            var studentViewModel = _mapper.Map<StudentViewModel>(student);
            return View(studentViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _unitOfWork.StudentsRepository.GetAllAsync() == null)
            {
                return Problem("Entity set 'CoursesDbContext.Students'  is null.");
            }

            var student = await _unitOfWork.StudentsRepository.GetByIdAsync(id);
            _unitOfWork.StudentsRepository.Remove(student);

            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
