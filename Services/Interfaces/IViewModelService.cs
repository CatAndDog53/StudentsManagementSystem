using System.Linq.Expressions;

namespace Services.Interfaces
{
    public interface IViewModelService<TViewModel>  where TViewModel : class
    {
        Task<TViewModel> GetByIdAsync(int? id);
        Task<IEnumerable<TViewModel>> GetAllAsync();
        Task Add(TViewModel viewModel);
        Task Update(TViewModel viewModel);
        Task Remove(TViewModel viewModel);
    }
}
