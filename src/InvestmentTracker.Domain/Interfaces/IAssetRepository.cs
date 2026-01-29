using InvestmentTracker.Domain.Entities;

namespace InvestmentTracker.Domain.Interfaces
{
    public interface IAssetRepository
    {
        Task<Asset?> GetByIdAsync(int id);
        Task<IEnumerable<Asset>> GetAllAsync(string? tag = null);
        Task<Asset> AddAsync(Asset asset);
        Task UpdateAsync(Asset asset);
        Task DeleteAsync(int id);
    }
}
