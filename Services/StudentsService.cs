using AutoMapper;
using Model;
using Services.Interfaces;
using ViewModels;

namespace Services
{
    public class StudentsService : IStudentsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudentViewModel> GetByIdAsync(int? id)
        {
            var student = await _unitOfWork.StudentsRepository.GetByIdAsync(id);
            return _mapper.Map<StudentViewModel>(student);
        }

        public async Task<IEnumerable<StudentViewModel>> GetAllAsync()
        {
            var students = await _unitOfWork.StudentsRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentViewModel>>(students);
        }

        public async Task Add(StudentViewModel studentViewModel)
        {
            var student = _mapper.Map<Student>(studentViewModel);
            _unitOfWork.StudentsRepository.Add(student);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(StudentViewModel studentViewModel)
        {
            var student = _mapper.Map<Student>(studentViewModel);
            _unitOfWork.StudentsRepository.Update(student);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Remove(StudentViewModel studentViewModel)
        {
            var student = _mapper.Map<Student>(studentViewModel);
            _unitOfWork.StudentsRepository.Remove(student);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ICollection<StudentViewModel>> GetStudentsByGroupIdAsync(int? groupId)
        {
            var students = await _unitOfWork.StudentsRepository.GetStudentsByGroupIdAsync(groupId);
            return _mapper.Map<IEnumerable<StudentViewModel>>(students).ToList();
        }

        public async Task<bool> StudentExists(int id)
        {
            if (await _unitOfWork.StudentsRepository.GetByIdAsync(id) == null)
            {
                return false;
            }
            return true;
        }
    }
}
