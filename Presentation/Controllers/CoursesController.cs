using Microsoft.AspNetCore.Mvc;
using ViewModels;
using Services.Interfaces;

namespace Presentation.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICoursesService _coursesService;
        private readonly IGroupsService _groupsService;

        public CoursesController(ICoursesService coursesService, IGroupsService groupsService)
        {
            _coursesService = coursesService;
            _groupsService = groupsService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _coursesService.GetAllAsync();
            return View(courses);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var course = await _coursesService.GetByIdAsync(id);
            if (course == null)
                return NotFound();
            return View(course);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Name,Description")] CourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                await _coursesService.Add(course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var course = await _coursesService.GetByIdAsync(id);
            if (course == null)
                return NotFound();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Name,Description")] CourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                await _coursesService.Update(course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var course = await _coursesService.GetByIdAsync(id);
            if (course == null)
                return NotFound();
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groups = await _groupsService.GetGroupsByCourseIdAsync(id);
            if (groups.Count() > 0)
            {
                return RedirectToAction(nameof(DeleteCourseWithGroupsError), new { courseId = id });
            }

            var course = await _coursesService.GetByIdAsync(id);
            await _coursesService.Remove(course);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteCourseWithGroupsError(int courseId)
        {
            var course = await _coursesService.GetByIdAsync(courseId);
            return View(course);
        }
    }
}
