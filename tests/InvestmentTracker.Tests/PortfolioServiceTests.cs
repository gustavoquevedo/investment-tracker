using Xunit;
using Moq;
using InvestmentTracker.Domain.Services;
using InvestmentTracker.Domain.Interfaces;
using InvestmentTracker.Domain.Entities;
using InvestmentTracker.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace InvestmentTracker.Tests
{
    public class PortfolioServiceTests
    {
        private readonly Mock<IAssetRepository> _mockAssetRepository;
        private readonly Mock<IContributionRepository> _mockContributionRepository;
        private readonly Mock<ISnapshotRepository> _mockSnapshotRepository;
        private readonly Mock<IFeeCalculator> _mockFeeCalculator;
        private readonly PortfolioService _service;

        public PortfolioServiceTests()
        {
            _mockAssetRepository = new Mock<IAssetRepository>();
            _mockContributionRepository = new Mock<IContributionRepository>();
            _mockSnapshotRepository = new Mock<ISnapshotRepository>();
            _mockFeeCalculator = new Mock<IFeeCalculator>();
            
            _service = new PortfolioService(
                _mockAssetRepository.Object, 
                _mockContributionRepository.Object, 
                _mockSnapshotRepository.Object,
                _mockFeeCalculator.Object);
        }

        [Fact]
        public async Task CreateAssetAsync_ShouldThrow_IfNameIsEmpty()
        {
            // Arrange
            var asset = new Asset { Name = "" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAssetAsync(asset));
        }

        [Fact]
        public async Task CreateAssetAsync_ShouldCallRepository_IfValid()
        {
            // Arrange
            var asset = new Asset { Name = "Test Asset", AssetType = AssetType.Stock };
            _mockAssetRepository.Setup(r => r.AddAsync(It.IsAny<Asset>())).ReturnsAsync(asset);

            // Act
            await _service.CreateAssetAsync(asset);

            // Assert
            _mockAssetRepository.Verify(r => r.AddAsync(asset), Times.Once);
        }

        [Fact]
        public async Task AddContributionAsync_ShouldThrow_IfAssetNotFound()
        {
            // Arrange
            int assetId = 1;
            var contribution = new Contribution { Amount = 100 };
            _mockAssetRepository.Setup(r => r.GetByIdAsync(assetId)).ReturnsAsync((Asset?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AddContributionAsync(assetId, contribution));
        }

        [Fact]
        public async Task AddContributionAsync_ShouldAddContribution_IfAssetExists()
        {
            // Arrange
            int assetId = 1;
            var asset = new Asset { Id = assetId, Name = "Test" };
            var contribution = new Contribution { Amount = 100 };

            _mockAssetRepository.Setup(r => r.GetByIdAsync(assetId)).ReturnsAsync(asset);

            // Act
            await _service.AddContributionAsync(assetId, contribution);

            // Assert
            Assert.Equal(assetId, contribution.AssetId);
            _mockContributionRepository.Verify(r => r.AddAsync(contribution), Times.Once);
        }

        [Fact]
        public async Task GetPortfolioSummaryAsync_ShouldCalculateTotalsCorrectly()
        {
            // Arrange
            var asset1 = new Asset { Id = 1, Name = "Asset 1" };
            var asset2 = new Asset { Id = 2, Name = "Asset 2" };
            var assets = new List<Asset> { asset1, asset2 };

            _mockAssetRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(assets);

            // Asset 1: 1000 invested, 1100 value
            _mockContributionRepository.Setup(r => r.GetByAssetIdAsync(1))
                .ReturnsAsync(new List<Contribution> { new Contribution { Amount = 1000 } });
            _mockSnapshotRepository.Setup(r => r.GetLatestByAssetIdAsync(1))
                .ReturnsAsync(new Snapshot { TotalValue = 1100 });

            // Asset 2: 500 invested, 400 value
             _mockContributionRepository.Setup(r => r.GetByAssetIdAsync(2))
                .ReturnsAsync(new List<Contribution> { new Contribution { Amount = 500 } });
            _mockSnapshotRepository.Setup(r => r.GetLatestByAssetIdAsync(2))
                .ReturnsAsync(new Snapshot { TotalValue = 400 });

            // Act
            var summary = await _service.GetPortfolioSummaryAsync();

            // Assert
            Assert.Equal(1500m, summary.TotalInvested);
            Assert.Equal(1500m, summary.TotalValue);
            Assert.Equal(0m, summary.TotalPnL);
        }
    }
}
