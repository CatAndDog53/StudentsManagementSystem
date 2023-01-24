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
            return View(await _unitOfWork.GroupsRepository.GetAllAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _unitOfWork.GroupsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var @group = await _unitOfWork.GroupsRepository.GetByIdAsync(id.GetValueOrDefault());
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Courses = new SelectList(await _unitOfWork.CoursesRepository.GetAllAsync(), "CourseId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,CourseId,Name")] Group @group)
        {
            ViewBag.Courses = new SelectList(await _unitOfWork.CoursesRepository.GetAllAsync(), "CourseId", "Name");

            if (ModelState.IsValid)
            {
                _unitOfWork.GroupsRepository.Add(@group);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@group);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Courses = new SelectList(await _unitOfWork.CoursesRepository.GetAllAsync(), "CourseId", "Name");

            if (id == null || await _unitOfWork.GroupsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var @group = await _unitOfWork.GroupsRepository.GetByIdAsync(id.GetValueOrDefault());
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
            ViewBag.Courses = new SelectList(await _unitOfWork.CoursesRepository.GetAllAsync(), "CourseId", "Name");

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
                    if (!await GroupExists(@group.GroupId))
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
            if (id == null || await _unitOfWork.GroupsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var @group = await _unitOfWork.GroupsRepository.GetByIdAsync(id.GetValueOrDefault());
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
            if (await _unitOfWork.GroupsRepository.GetAllAsync() == null)
            {
                return Problem("Entity set 'CoursesDbContext.Groups'  is null.");
            }

            var students = await _unitOfWork.StudentsRepository.GetStudentsByGroupIdAsync(id);
            if (students.Count > 0)
            {
                return RedirectToAction("DeleteGroupWithStudentsError", new { groupId = id });
            }

            Group group = await _unitOfWork.GroupsRepository.GetByIdAsync(id);
            _unitOfWork.GroupsRepository.Remove(group);

            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteGroupWithStudentsError(int groupId)
        {
            Group group = await _unitOfWork.GroupsRepository.GetByIdAsync(groupId);
            return View(group);
        }

        private async Task<bool> GroupExists(int id)
        {
            if (await _unitOfWork.GroupsRepository.GetByIdAsync(id) == null)
            {
                return false;
            }   
            return true;
        }
    }
}
