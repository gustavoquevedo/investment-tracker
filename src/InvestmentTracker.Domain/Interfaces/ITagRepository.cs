using InvestmentTracker.Domain.Entities;

namespace InvestmentTracker.Domain.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag?> GetByIdAsync(int id);
        Task<Tag> AddAsync(Tag tag);
        Task DeleteAsync(int id);
    }
}
