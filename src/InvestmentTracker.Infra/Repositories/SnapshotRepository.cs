using InvestmentTracker.Domain.Entities;
using InvestmentTracker.Domain.Interfaces;
using InvestmentTracker.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace InvestmentTracker.Infra.Repositories
{
    public class SnapshotRepository : ISnapshotRepository
    {
        private readonly InvestmentContext _context;

        public SnapshotRepository(InvestmentContext context)
        {
            _context = context;
        }

        public async Task<Snapshot?> GetLatestByAssetIdAsync(int assetId)
        {
            return await _context.Snapshots
                .Where(s => s.AssetId == assetId)
                .OrderByDescending(s => s.SnapshotDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Snapshot>> GetByAssetIdAsync(int assetId)
        {
            return await _context.Snapshots
                .Where(s => s.AssetId == assetId)
                .OrderByDescending(s => s.SnapshotDate)
                .ToListAsync();
        }

        public async Task<Snapshot> AddAsync(Snapshot snapshot)
        {
            _context.Snapshots.Add(snapshot);
            await _context.SaveChangesAsync();
            return snapshot;
        }
    }
}
