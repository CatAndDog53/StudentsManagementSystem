namespace Services.Interfaces
{
    public interface IStudentsService
    {
        Task<bool> StudentExists(int id);
    }
}
