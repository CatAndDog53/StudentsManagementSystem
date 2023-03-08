using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModels;
using Services.Interfaces;

namespace Presentation.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentsService _studentsService;
        private readonly IGroupsService _groupsService;

        public StudentsController(IStudentsService studentsService, IGroupsService groupsService)
        {
            _studentsService= studentsService;
            _groupsService = groupsService;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _studentsService.GetAllAsync();
            return View(students);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var student = await _studentsService.GetByIdAsync(id.GetValueOrDefault());
            if (student == null)
                return NotFound();
            return View(student);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Groups = new SelectList(await _groupsService.GetAllAsync(), "GroupId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,GroupId,FirstName,LastName")] StudentViewModel student)
        {
            ViewBag.Groups = new SelectList(await _groupsService.GetAllAsync(), "GroupId", "Name");

            if (ModelState.IsValid)
            {
                await _studentsService.Add(student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Groups = new SelectList(await _groupsService.GetAllAsync(), "GroupId", "Name");

            var student = await _studentsService.GetByIdAsync(id.GetValueOrDefault());
            if (student == null)
                return NotFound();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,GroupId,FirstName,LastName")] StudentViewModel student)
        {
            ViewBag.Groups = new SelectList(await _groupsService.GetAllAsync(), "GroupId", "Name");

            if (ModelState.IsValid)
            {
                await _studentsService.Update(student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var student = await _studentsService.GetByIdAsync(id);
            if (student == null)
                return NotFound();
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _studentsService.GetByIdAsync(id);
            await _studentsService.Remove(student);
            return RedirectToAction(nameof(Index));
        }
    }
}
