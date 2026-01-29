using Microsoft.AspNetCore.Mvc;
using InvestmentTracker.Domain.Interfaces;
using InvestmentTracker.Domain.Entities;
using InvestmentTracker.Api.DTOs;
using InvestmentTracker.Domain.Enums;

namespace InvestmentTracker.Api.Controllers
{
    [ApiController]
    [Route("assets")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ISnapshotRepository _snapshotRepository;
        private readonly IContributionRepository _contributionRepository;
        private readonly IPortfolioService _portfolioService;

        public AssetsController(
            IAssetRepository assetRepository, 
            ISnapshotRepository snapshotRepository,
            IContributionRepository contributionRepository,
            IPortfolioService portfolioService)
        {
            _assetRepository = assetRepository;
            _snapshotRepository = snapshotRepository;
            _contributionRepository = contributionRepository;
            _portfolioService = portfolioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetResponse>>> GetAll([FromQuery] string? tag = null)
        {
            var assets = await _assetRepository.GetAllAsync(tag);
            return Ok(assets.Select(MapToResponse));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssetResponse>> GetById(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null) return NotFound();
            return Ok(MapToResponse(asset));
        }

        [HttpPost]
        public async Task<ActionResult<AssetResponse>> Create(CreateAssetRequest request)
        {
            if (!Enum.TryParse<AssetType>(request.AssetType, true, out var assetType))
            {
                return BadRequest($"Invalid AssetType. Valid values: {string.Join(", ", Enum.GetNames(typeof(AssetType)))}");
            }

            var asset = new Asset
            {
                Name = request.Name,
                AssetType = assetType,
                Isin = request.ISIN,
                Ticker = request.Ticker,
                FeePercentagePerYear = request.FeePercentagePerYear
            };

            var createdAsset = await _portfolioService.CreateAssetAsync(asset);
            return CreatedAtAction(nameof(GetById), new { id = createdAsset.Id }, MapToResponse(createdAsset));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AssetResponse>> Update(int id, UpdateAssetRequest request)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null) return NotFound();

            if (request.Name != null) asset.Name = request.Name;
            if (request.ISIN != null) asset.Isin = request.ISIN;
            if (request.Ticker != null) asset.Ticker = request.Ticker;
            if (request.FeePercentagePerYear.HasValue) asset.FeePercentagePerYear = request.FeePercentagePerYear.Value;
            
            if (request.AssetType != null)
            {
                 if (!Enum.TryParse<AssetType>(request.AssetType, true, out var assetType))
                {
                    return BadRequest($"Invalid AssetType. Valid values: {string.Join(", ", Enum.GetNames(typeof(AssetType)))}");
                }
                asset.AssetType = assetType;
            }

            await _assetRepository.UpdateAsync(asset);
            return Ok(MapToResponse(asset));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null) return NotFound();
            await _assetRepository.DeleteAsync(id);
            return NoContent();
        }

        // --- Sub-resources ---

        [HttpGet("{id}/snapshots")]
        public async Task<ActionResult<IEnumerable<object>>> GetSnapshots(int id)
        {
             var asset = await _assetRepository.GetByIdAsync(id);
             if (asset == null) return NotFound();

             var snapshots = await _snapshotRepository.GetByAssetIdAsync(id);
             return Ok(snapshots.Select(s => new {
                 s.Id, s.AssetId, s.TotalValue, s.SnapshotDate
             }));
        }

        [HttpPost("{id}/snapshots")]
        public async Task<ActionResult> AddSnapshot(int id, CreateSnapshotRequest request)
        {
             var asset = await _assetRepository.GetByIdAsync(id);
             if (asset == null) return NotFound();

             var snapshot = new Snapshot 
             { 
                 AssetId = id, 
                 TotalValue = request.TotalValue, 
                 SnapshotDate = request.SnapshotDate 
             };
             
             await _portfolioService.AddSnapshotAsync(id, snapshot); 
             return CreatedAtAction(nameof(GetSnapshots), new { id }, null); 
        }

        [HttpGet("{id}/contributions")]
        public async Task<ActionResult<IEnumerable<object>>> GetContributions(int id)
        {
             var asset = await _assetRepository.GetByIdAsync(id);
             if (asset == null) return NotFound();

             var contributions = await _contributionRepository.GetByAssetIdAsync(id);
             return Ok(contributions.Select(c => new {
                 c.Id, c.AssetId, c.Amount, c.DateMade, c.Note
             }));
        }

        [HttpPost("{id}/contributions")]
        public async Task<ActionResult> AddContribution(int id, CreateContributionRequest request)
        {
             var asset = await _assetRepository.GetByIdAsync(id);
             if (asset == null) return NotFound();

             var contribution = new Contribution
             {
                 AssetId = id,
                 Amount = request.Amount,
                 DateMade = request.DateMade,
                 Note = request.Note
             };

             await _portfolioService.AddContributionAsync(id, contribution);
             return CreatedAtAction(nameof(GetContributions), new { id }, null);
        }

        private static AssetResponse MapToResponse(Asset asset)
        {
            return new AssetResponse
            {
                Id = asset.Id,
                Name = asset.Name,
                AssetType = asset.AssetType.ToString(),
                ISIN = asset.Isin,
                Ticker = asset.Ticker,
                FeePercentagePerYear = asset.FeePercentagePerYear,
                CreatedAt = asset.CreatedAt,
                Tags = asset.AssetTags.Select(at => new TagDto
                {
                    Id = at.Tag.Id,
                    Name = at.Tag.Name,
                    Color = at.Tag.ColorHex
                }).ToList()
            };
        }
    }
}
