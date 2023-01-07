using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using Services;

namespace Presentation.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.StudentsRepository.GetAllStudentsAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _unitOfWork.StudentsRepository.GetAllStudentsAsync() == null)
            {
                return NotFound();
            }

            var student = await _unitOfWork.StudentsRepository.GetStudentByIdAsync(id.GetValueOrDefault());
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        public IActionResult Create()
        {
            ViewBag.Groups = new SelectList(_unitOfWork.GroupsRepository.GetAllGroupsAsync().Result, "GroupId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,GroupId,FirstName,LastName")] Student student)
        {
            ViewBag.Groups = new SelectList(_unitOfWork.GroupsRepository.GetAllGroupsAsync().Result, "GroupId", "Name");

            if (ModelState.IsValid)
            {
                _unitOfWork.StudentsRepository.Insert(student);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Groups = new SelectList(_unitOfWork.GroupsRepository.GetAllGroupsAsync().Result, "GroupId", "Name");

            if (id == null || await _unitOfWork.StudentsRepository.GetAllStudentsAsync() == null)
            {
                return NotFound();
            }

            var student = await _unitOfWork.StudentsRepository.GetStudentByIdAsync(id.GetValueOrDefault());
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName")] Student student)
        {
            ViewBag.Groups = new SelectList(_unitOfWork.GroupsRepository.GetAllGroupsAsync().Result, "GroupId", "Name");

            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.StudentsRepository.Update(student);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            return View(student);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _unitOfWork.StudentsRepository.GetAllStudentsAsync() == null)
            {
                return NotFound();
            }

            var student = await _unitOfWork.StudentsRepository.GetStudentByIdAsync(id.GetValueOrDefault());
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _unitOfWork.StudentsRepository.GetAllStudentsAsync() == null)
            {
                return Problem("Entity set 'CoursesDbContext.Students'  is null.");
            }

            _unitOfWork.StudentsRepository.Delete(id);

            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            if (_unitOfWork.StudentsRepository.GetAllStudentsAsync().Result == null)
                return false;
            return true;
        }
    }
}
