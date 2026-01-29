using InvestmentTracker.Domain.Entities;

namespace InvestmentTracker.Domain.Interfaces
{
    public interface IContributionRepository
    {
        Task<IEnumerable<Contribution>> GetByAssetIdAsync(int assetId);
        Task<Contribution> AddAsync(Contribution contribution);
    }
}
