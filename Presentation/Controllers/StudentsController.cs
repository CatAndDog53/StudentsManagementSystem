using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using Services;

namespace Presentation.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentsService _studentsService;
        private readonly IGroupsService _groupsService;

        public StudentsController(IStudentsService studentsService, IGroupsService groupsService)
        {
            _studentsService = studentsService;
            _groupsService = groupsService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _studentsService.GetAllStudentsAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _studentsService.GetAllStudentsAsync() == null)
            {
                return NotFound();
            }

            var student = await _studentsService.GetStudentByIdAsync(id.GetValueOrDefault());
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        public IActionResult Create()
        {
            ViewBag.Groups = new SelectList(_groupsService.GetAllGroupsAsync().Result, "GroupId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,GroupId,FirstName,LastName")] Student student)
        {
            ViewBag.Groups = new SelectList(_groupsService.GetAllGroupsAsync().Result, "GroupId", "Name");

            if (ModelState.IsValid)
            {
                _studentsService.Insert(student);
                await _studentsService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Groups = new SelectList(_groupsService.GetAllGroupsAsync().Result, "GroupId", "Name");

            if (id == null || await _studentsService.GetAllStudentsAsync() == null)
            {
                return NotFound();
            }

            var student = await _studentsService.GetStudentByIdAsync(id.GetValueOrDefault());
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
            ViewBag.Groups = new SelectList(_groupsService.GetAllGroupsAsync().Result, "GroupId", "Name");

            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _studentsService.Update(student);
                    await _studentsService.SaveChangesAsync();
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
            if (id == null || await _studentsService.GetAllStudentsAsync() == null)
            {
                return NotFound();
            }

            var student = await _studentsService.GetStudentByIdAsync(id.GetValueOrDefault());
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
            if (await _studentsService.GetAllStudentsAsync() == null)
            {
                return Problem("Entity set 'CoursesDbContext.Students'  is null.");
            }

            _studentsService.Delete(id);

            await _studentsService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            if (_studentsService.GetAllStudentsAsync().Result == null)
                return false;
            return true;
        }
    }
}
