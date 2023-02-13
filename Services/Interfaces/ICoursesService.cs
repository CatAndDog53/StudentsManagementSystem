namespace Services.Interfaces
{
    public interface ICoursesService
    {
        Task<bool> CourseExists(int id);
    }
}
