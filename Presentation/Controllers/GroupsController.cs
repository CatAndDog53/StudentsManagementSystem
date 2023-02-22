using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using ViewModels;
using Services;
using Services.Interfaces;

namespace Presentation.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGroupsService _groupsService;
        private readonly IMapper _mapper;

        public GroupsController(IUnitOfWork unitOfWork, IGroupsService groupsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _groupsService = groupsService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var groupsViewModel = _mapper.Map<IEnumerable<GroupViewModel>>(await _unitOfWork.GroupsRepository.GetAllAsync());
            return View(groupsViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _unitOfWork.GroupsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var group = await _unitOfWork.GroupsRepository.GetByIdAsync(id.GetValueOrDefault());
            if (group == null)
            {
                return NotFound();
            }

            var groupViewModel = _mapper.Map<GroupViewModel>(group);
            return View(groupViewModel);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Courses = new SelectList(
                _mapper.Map<IEnumerable<CourseViewModel>>(await _unitOfWork.CoursesRepository.GetAllAsync()), 
                "CourseId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,CourseId,Name")] GroupViewModel groupViewModel)
        {
            ViewBag.Courses = new SelectList(
                _mapper.Map<IEnumerable<CourseViewModel>>(await _unitOfWork.CoursesRepository.GetAllAsync()),
                "CourseId", "Name");

            if (ModelState.IsValid)
            {
                _unitOfWork.GroupsRepository.Add(_mapper.Map<Group>(groupViewModel));
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groupViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Courses = new SelectList(
                _mapper.Map<IEnumerable<CourseViewModel>>(await _unitOfWork.CoursesRepository.GetAllAsync()),
                "CourseId", "Name");

            if (id == null || await _unitOfWork.GroupsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var group = await _unitOfWork.GroupsRepository.GetByIdAsync(id.GetValueOrDefault());
            if (group == null)
            {
                return NotFound();
            }

            var groupViewModel = _mapper.Map<GroupViewModel>(group);
            return View(groupViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupId,CourseId,Name")] GroupViewModel groupViewModel)
        {
            ViewBag.Courses = new SelectList(
                _mapper.Map<IEnumerable<CourseViewModel>>(await _unitOfWork.CoursesRepository.GetAllAsync()),
                "CourseId", "Name");

            if (id != groupViewModel.GroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var group = _mapper.Map<Group>(groupViewModel);
                try
                {
                    _unitOfWork.GroupsRepository.Update(group);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _groupsService.GroupExists(group.GroupId))
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
            return View(groupViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _unitOfWork.GroupsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var group = await _unitOfWork.GroupsRepository.GetByIdAsync(id.GetValueOrDefault());
            if (group == null)
            {
                return NotFound();
            }

            var groupViewModel = _mapper.Map<GroupViewModel>(group);
            return View(groupViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var students = await _unitOfWork.StudentsRepository.GetStudentsByGroupIdAsync(id);
            if (students.Count > 0)
            {
                return RedirectToAction(nameof(DeleteGroupWithStudentsError), new { groupId = id });
            }

            var group = await _unitOfWork.GroupsRepository.GetByIdAsync(id);
            _unitOfWork.GroupsRepository.Remove(group);

            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteGroupWithStudentsError(int groupId)
        {
            var group = await _unitOfWork.GroupsRepository.GetByIdAsync(groupId);
            var groupViewModel = _mapper.Map<GroupViewModel>(group);
            return View(groupViewModel);
        }
    }
}
