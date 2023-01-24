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
            return View(await _unitOfWork.CoursesRepository.GetAllAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _unitOfWork.CoursesRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.CoursesRepository.GetByIdAsync(id.GetValueOrDefault());
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
                _unitOfWork.CoursesRepository.Add(course);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _unitOfWork.CoursesRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.CoursesRepository.GetByIdAsync(id.GetValueOrDefault());
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
                    if (!await CourseExists(course.CourseId))
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
            if (id == null || await _unitOfWork.CoursesRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.CoursesRepository.GetByIdAsync(id.GetValueOrDefault());
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
            if (await _unitOfWork.CoursesRepository.GetAllAsync() == null)
            {
                return Problem("Entity set 'CoursesDbContext.Courses'  is null.");
            }

            var groups = await _unitOfWork.GroupsRepository.GetGroupsByCourseIdAsync(id);
            if (groups.Count > 0)
            {
                return RedirectToAction("DeleteCourseWithGroupsError", new { courseId = id });
            }

            Course course = await _unitOfWork.CoursesRepository.GetByIdAsync(id);
            _unitOfWork.CoursesRepository.Remove(course);

            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteCourseWithGroupsError(int courseId)
        {
            Course course = await _unitOfWork.CoursesRepository.GetByIdAsync(courseId);
            return View(course);
        }

        private async Task<bool> CourseExists(int id)
        {
            if (await _unitOfWork.CoursesRepository.GetByIdAsync(id) == null)
            {
                return false;
            }
            return true;
        }
    }
}
