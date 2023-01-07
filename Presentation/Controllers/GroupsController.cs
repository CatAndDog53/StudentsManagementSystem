using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using Services;

namespace Presentation.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GroupsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.GroupsRepository.GetAllGroupsAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _unitOfWork.GroupsRepository.GetAllGroupsAsync() == null)
            {
                return NotFound();
            }

            var @group = await _unitOfWork.GroupsRepository.GetGroupByIdAsync(id.GetValueOrDefault());
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        public IActionResult Create()
        {
            ViewBag.Courses = new SelectList(_unitOfWork.CoursesRepository.GetAllCoursesAsync().Result, "CourseId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,CourseId,Name")] Group @group)
        {
            ViewBag.Courses = new SelectList(_unitOfWork.CoursesRepository.GetAllCoursesAsync().Result, "CourseId", "Name");

            if (ModelState.IsValid)
            {
                _unitOfWork.GroupsRepository.Insert(@group);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@group);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Courses = new SelectList(_unitOfWork.CoursesRepository.GetAllCoursesAsync().Result, "CourseId", "Name");

            if (id == null || await _unitOfWork.GroupsRepository.GetAllGroupsAsync() == null)
            {
                return NotFound();
            }

            var @group = await _unitOfWork.GroupsRepository.GetGroupByIdAsync(id.GetValueOrDefault());
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
            ViewBag.Courses = new SelectList(_unitOfWork.CoursesRepository.GetAllCoursesAsync().Result, "CourseId", "Name");

            if (id != @group.GroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.GroupsRepository.Update(@group);
                    await _unitOfWork.SaveChangesAsync();
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
            if (id == null || await _unitOfWork.GroupsRepository.GetAllGroupsAsync() == null)
            {
                return NotFound();
            }

            var @group = await _unitOfWork.GroupsRepository.GetGroupByIdAsync(id.GetValueOrDefault());
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
            if (await _unitOfWork.GroupsRepository.GetAllGroupsAsync() == null)
            {
                return Problem("Entity set 'CoursesDbContext.Groups'  is null.");
            }

            _unitOfWork.GroupsRepository.Delete(id);

            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            if (_unitOfWork.GroupsRepository.GetAllGroupsAsync().Result == null)
                return false;
            return true;
        }
    }
}
