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
            return View(await _unitOfWork.StudentsRepository.GetAllAsync());
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

            return View(student);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Groups = new SelectList(await _unitOfWork.GroupsRepository.GetAllAsync(), "GroupId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,GroupId,FirstName,LastName")] Student student)
        {
            ViewBag.Groups = new SelectList(await _unitOfWork.GroupsRepository.GetAllAsync(), "GroupId", "Name");

            if (ModelState.IsValid)
            {
                _unitOfWork.StudentsRepository.Add(student);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Groups = new SelectList(await _unitOfWork.GroupsRepository.GetAllAsync(), "GroupId", "Name");

            if (id == null || await _unitOfWork.StudentsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var student = await _unitOfWork.StudentsRepository.GetByIdAsync(id.GetValueOrDefault());
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,GroupId,FirstName,LastName")] Student student)
        {
            ViewBag.Groups = new SelectList(await _unitOfWork.GroupsRepository.GetAllAsync(), "GroupId", "Name");

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
                    if (!await StudentExists(student.StudentId))
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
            if (id == null || await _unitOfWork.StudentsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var student = await _unitOfWork.StudentsRepository.GetByIdAsync(id.GetValueOrDefault());
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
            if (await _unitOfWork.StudentsRepository.GetAllAsync() == null)
            {
                return Problem("Entity set 'CoursesDbContext.Students'  is null.");
            }

            Student student = await _unitOfWork.StudentsRepository.GetByIdAsync(id);
            _unitOfWork.StudentsRepository.Remove(student);

            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> StudentExists(int id)
        {
            if (await _unitOfWork.StudentsRepository.GetAllAsync() == null)
                return false;
            return true;
        }
    }
}
