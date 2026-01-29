using InvestmentTracker.Domain.Entities;
using InvestmentTracker.Domain.Interfaces;
using InvestmentTracker.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace InvestmentTracker.Infra.Repositories
{
    public class ContributionRepository : IContributionRepository
    {
        private readonly InvestmentContext _context;

        public ContributionRepository(InvestmentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contribution>> GetByAssetIdAsync(int assetId)
        {
            return await _context.Contributions
                .Where(c => c.AssetId == assetId)
                .ToListAsync();
        }

        public async Task<Contribution> AddAsync(Contribution contribution)
        {
            _context.Contributions.Add(contribution);
            await _context.SaveChangesAsync();
            return contribution;
        }
    }
}
