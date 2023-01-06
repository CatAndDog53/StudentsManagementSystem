using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Services;

namespace Presentation.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICoursesService _coursesService;

        public CoursesController(ICoursesService coursesService)
        {
            _coursesService = coursesService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _coursesService.GetAllCoursesAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _coursesService.GetAllCoursesAsync() == null)
            {
                return NotFound();
            }

            var course = await _coursesService.GetCourseByIdAsync(id.GetValueOrDefault());
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
                _coursesService.Insert(course);
                await _coursesService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _coursesService.GetAllCoursesAsync == null)
            {
                return NotFound();
            }

            var course = await _coursesService.GetCourseByIdAsync(id.GetValueOrDefault());
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
                    _coursesService.Update(course);
                    await _coursesService.SaveChangesAsync();
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
            if (id == null || _coursesService.GetAllCoursesAsync() == null)
            {
                return NotFound();
            }

            var course = await _coursesService.GetCourseByIdAsync(id.GetValueOrDefault());
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
            if (_coursesService.GetAllCoursesAsync() == null)
            {
                return Problem("Entity set 'CoursesDbContext.Courses'  is null.");
            }

            _coursesService.Delete(id);

            await _coursesService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            if (_coursesService.GetAllCoursesAsync() == null)
                return false;
            return true;
        }
    }
}
