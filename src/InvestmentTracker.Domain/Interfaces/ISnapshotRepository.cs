using InvestmentTracker.Domain.Entities;

namespace InvestmentTracker.Domain.Interfaces
{
    public interface ISnapshotRepository
    {
        Task<Snapshot?> GetLatestByAssetIdAsync(int assetId);
        Task<IEnumerable<Snapshot>> GetByAssetIdAsync(int assetId);
        Task<Snapshot> AddAsync(Snapshot snapshot);
    }
}
