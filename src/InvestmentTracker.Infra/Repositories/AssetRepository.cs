using InvestmentTracker.Domain.Entities;
using InvestmentTracker.Domain.Interfaces;
using InvestmentTracker.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace InvestmentTracker.Infra.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly InvestmentContext _context;

        public AssetRepository(InvestmentContext context)
        {
            _context = context;
        }

        public async Task<Asset?> GetByIdAsync(int id)
        {
            return await _context.Assets
                .Include(a => a.AssetTags)
                .ThenInclude(at => at.Tag)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Asset>> GetAllAsync(string? tag = null)
        {
            var query = _context.Assets
                .Include(a => a.AssetTags)
                .ThenInclude(at => at.Tag)
                .AsQueryable();

            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(a => a.AssetTags.Any(at => at.Tag.Name == tag));
            }

            return await query.ToListAsync();
        }

        public async Task<Asset> AddAsync(Asset asset)
        {
            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();
            return asset;
        }

        public async Task UpdateAsync(Asset asset)
        {
            _context.Entry(asset).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var asset = await GetByIdAsync(id);
            if (asset != null)
            {
                _context.Assets.Remove(asset);
                await _context.SaveChangesAsync();
            }
        }
    }
}
