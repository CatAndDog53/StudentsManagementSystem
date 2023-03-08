using AutoMapper;
using Model;
using Services.Interfaces;
using ViewModels;

namespace Services
{
    public class CoursesService : ICoursesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CoursesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CourseViewModel> GetByIdAsync(int? id)
        {
            var course = await _unitOfWork.CoursesRepository.GetByIdAsync(id);
            return _mapper.Map<CourseViewModel>(course);
        }

        public async Task<IEnumerable<CourseViewModel>> GetAllAsync()
        {
            var courses = await _unitOfWork.CoursesRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseViewModel>>(courses);
        }

        public async Task Add(CourseViewModel courseViewModel)
        {
            var course = _mapper.Map<Course>(courseViewModel);
            _unitOfWork.CoursesRepository.Add(course);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(CourseViewModel courseViewModel)
        {
            var course = _mapper.Map<Course>(courseViewModel);
            _unitOfWork.CoursesRepository.Update(course);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Remove(CourseViewModel courseViewModel)
        {
            var course = _mapper.Map<Course>(courseViewModel);
            _unitOfWork.CoursesRepository.Remove(course);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> CourseExists(int id)
        {
            if (await _unitOfWork.CoursesRepository.GetByIdAsync(id) == null)
            {
                return false;
            }
            return true;
        }
    }
}
