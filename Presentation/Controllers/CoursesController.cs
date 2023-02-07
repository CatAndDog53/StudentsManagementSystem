using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Presentation.ViewModels;
using Services;

namespace Presentation.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CoursesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var coursesViewModel = _mapper.Map<IEnumerable<CourseViewModel>>(await _unitOfWork.CoursesRepository.GetAllAsync());
            return View(coursesViewModel);
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

            var courseViewModel = _mapper.Map<CourseViewModel>(course);
            return View(courseViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Name,Description")] CourseViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoursesRepository.Add(_mapper.Map<Course>(courseViewModel));
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(courseViewModel);
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

            var courseViewModel = _mapper.Map<CourseViewModel>(course);
            return View(courseViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Name,Description")] CourseViewModel courseViewModel)
        {
            if (id != courseViewModel.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Course>(courseViewModel);
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
            return View(courseViewModel);
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

            var courseViewModel = _mapper.Map<CourseViewModel>(course);

            return View(courseViewModel);
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
            var course = await _unitOfWork.CoursesRepository.GetByIdAsync(courseId);
            var courseViewModel = _mapper.Map<CourseViewModel>(course);
            return View(courseViewModel);
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
