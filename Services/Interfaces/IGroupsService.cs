namespace Services.Interfaces
{
    public interface IGroupsService
    {
        Task<bool> GroupExists(int id);
    }
}
