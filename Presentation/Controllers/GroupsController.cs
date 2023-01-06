using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using Services;

namespace Presentation.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IGroupsService _groupsService;
        private readonly ICoursesService _coursesService;

        public GroupsController(IGroupsService groupsService, ICoursesService coursesService)
        {
            _groupsService = groupsService;
            _coursesService = coursesService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _groupsService.GetAllGroupsAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _groupsService.GetAllGroupsAsync() == null)
            {
                return NotFound();
            }

            var @group = await _groupsService.GetGroupByIdAsync(id.GetValueOrDefault());
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        public IActionResult Create()
        {
            ViewBag.Courses = new SelectList(_coursesService.GetAllCoursesAsync().Result, "CourseId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,CourseId,Name")] Group @group)
        {
            ViewBag.Courses = new SelectList(_coursesService.GetAllCoursesAsync().Result, "CourseId", "Name");

            if (ModelState.IsValid)
            {
                _groupsService.Insert(@group);
                await _groupsService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@group);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Courses = new SelectList(_coursesService.GetAllCoursesAsync().Result, "CourseId", "Name");

            if (id == null || await _groupsService.GetAllGroupsAsync() == null)
            {
                return NotFound();
            }

            var @group = await _groupsService.GetGroupByIdAsync(id.GetValueOrDefault());
            if (@group == null)
            {
                return NotFound();
            }
            return View(@group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupId,CourseId,Name")] Group @group)
        {
            ViewBag.Courses = new SelectList(_coursesService.GetAllCoursesAsync().Result, "CourseId", "Name");

            if (id != @group.GroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _groupsService.Update(@group);
                    await _groupsService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.GroupId))
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
            return View(@group);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _groupsService.GetAllGroupsAsync() == null)
            {
                return NotFound();
            }

            var @group = await _groupsService.GetGroupByIdAsync(id.GetValueOrDefault());
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _groupsService.GetAllGroupsAsync() == null)
            {
                return Problem("Entity set 'CoursesDbContext.Groups'  is null.");
            }

            _groupsService.Delete(id);

            await _groupsService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            if (_groupsService.GetAllGroupsAsync().Result == null)
                return false;
            return true;
        }
    }
}
