using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Services;

namespace Presentation.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoursesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.CoursesRepository.GetAllCoursesAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _unitOfWork.CoursesRepository.GetAllCoursesAsync() == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.CoursesRepository.GetCourseByIdAsync(id.GetValueOrDefault());
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Name,Description")] Course course)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoursesRepository.Insert(course);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _unitOfWork.CoursesRepository.GetAllCoursesAsync == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.CoursesRepository.GetCourseByIdAsync(id.GetValueOrDefault());
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Name,Description")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.CoursesRepository.Update(course);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
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
            return View(course);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _unitOfWork.CoursesRepository.GetAllCoursesAsync() == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.CoursesRepository.GetCourseByIdAsync(id.GetValueOrDefault());
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_unitOfWork.CoursesRepository.GetAllCoursesAsync() == null)
            {
                return Problem("Entity set 'CoursesDbContext.Courses'  is null.");
            }

            _unitOfWork.CoursesRepository.Delete(id);

            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            if (_unitOfWork.CoursesRepository.GetAllCoursesAsync() == null)
                return false;
            return true;
        }
    }
}
