using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModels;
using Services.Interfaces;

namespace Presentation.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IGroupsService _groupsService;
        private readonly ICoursesService _coursesService;
        private readonly IStudentsService _studentsService;

        public GroupsController(IGroupsService groupsService, ICoursesService coursesService, IStudentsService studentsService)
        {
            _groupsService = groupsService;
            _coursesService = coursesService;
            _studentsService = studentsService;
        }

        public async Task<IActionResult> Index()
        {
            var groups = await _groupsService.GetAllAsync();
            return View(groups);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var group = await _groupsService.GetByIdAsync(id);
            if (group == null)
                return NotFound();
            return View(group);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Courses = new SelectList(await _coursesService.GetAllAsync(), "CourseId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,CourseId,Name")] GroupViewModel group)
        {
            ViewBag.Courses = new SelectList(await _coursesService.GetAllAsync(), "CourseId", "Name");

            if (ModelState.IsValid)
            {
                await _groupsService.Add(group);
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Courses = new SelectList(await _coursesService.GetAllAsync(), "CourseId", "Name");

            var group = await _groupsService.GetByIdAsync(id);
            if (group == null)
                return NotFound();
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupId,CourseId,Name")] GroupViewModel group)
        {
            ViewBag.Courses = new SelectList(await _coursesService.GetAllAsync(), "CourseId", "Name");

            if (ModelState.IsValid)
            {
                await _groupsService.Update(group);
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var group = await _groupsService.GetByIdAsync(id);
            if (group == null)
                return NotFound();
            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var students = await _studentsService.GetStudentsByGroupIdAsync(id);
            if (students.Count > 0)
            {
                return RedirectToAction(nameof(DeleteGroupWithStudentsError), new { groupId = id });
            }

            var group = await _groupsService.GetByIdAsync(id);
            await _groupsService.Remove(group);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteGroupWithStudentsError(int groupId)
        {
            var group = await _groupsService.GetByIdAsync(groupId);
            return View(group);
        }
    }
}
