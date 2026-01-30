using InvestmentTracker.Domain.DTOs;
using InvestmentTracker.Domain.Entities;

namespace InvestmentTracker.Domain.Interfaces
{
    public interface IPortfolioService
    {
        Task<Asset> CreateAssetAsync(Asset asset);
        Task AddContributionAsync(int assetId, Contribution contribution);
        Task AddSnapshotAsync(int assetId, Snapshot snapshot);
        Task<PortfolioSummary> GetPortfolioSummaryAsync();
        
        // New reporting methods
        Task<PortfolioHistory> GetHistoryAsync(DateOnly? from, DateOnly? to);
        Task<PortfolioAllocation> GetAllocationAsync();
        Task<PortfolioReturns> GetReturnsAsync(DateOnly? asOf);
    }
}
