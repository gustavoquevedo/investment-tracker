using Xunit;
using FluentAssertions;
using Moq;
using InvestmentTracker.Domain.Services;
using InvestmentTracker.Domain.Interfaces;
using InvestmentTracker.Domain.Entities;
using InvestmentTracker.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace InvestmentTracker.Tests;

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

        // Act
        var act = async () => await _service.CreateAssetAsync(asset);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
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

        // Act
        var act = async () => await _service.AddContributionAsync(assetId, contribution);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
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
        contribution.AssetId.Should().Be(assetId);
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

        // Asset 1: 1000 invested, 1100 value (no fee set)
        _mockContributionRepository.Setup(r => r.GetByAssetIdAsync(1))
            .ReturnsAsync(new List<Contribution> { new Contribution { Amount = 1000, DateMade = new DateTime(2023, 1, 1) } });
        _mockSnapshotRepository.Setup(r => r.GetLatestByAssetIdAsync(1))
            .ReturnsAsync(new Snapshot { TotalValue = 1100, SnapshotDate = new DateTime(2024, 1, 1) });

        // Asset 2: 500 invested, 400 value (no fee set)
        _mockContributionRepository.Setup(r => r.GetByAssetIdAsync(2))
            .ReturnsAsync(new List<Contribution> { new Contribution { Amount = 500, DateMade = new DateTime(2023, 1, 1) } });
        _mockSnapshotRepository.Setup(r => r.GetLatestByAssetIdAsync(2))
            .ReturnsAsync(new Snapshot { TotalValue = 400, SnapshotDate = new DateTime(2024, 1, 1) });

        // Act
        var summary = await _service.GetPortfolioSummaryAsync();

        // Assert
        summary.TotalInvested.Should().Be(1500m);
        summary.TotalValue.Should().Be(1500m);
        summary.TotalPnL.Should().Be(0m);
        summary.TotalFees.Should().Be(0m); // No fees since FeePercentagePerYear is 0
        summary.NetPnL.Should().Be(0m);
    }

    [Fact]
    public async Task GetPortfolioSummaryAsync_ShouldIncludeFees_WhenAssetsHaveFeePercentage()
    {
        // Arrange
        var asset1 = new Asset { Id = 1, Name = "Asset 1", FeePercentagePerYear = 0.01m }; // 1% fee
        var assets = new List<Asset> { asset1 };

        _mockAssetRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(assets);

        var contributionDate = new DateTime(2023, 1, 1);
        var snapshotDate = new DateTime(2024, 1, 1); // 365 days

        _mockContributionRepository.Setup(r => r.GetByAssetIdAsync(1))
            .ReturnsAsync(new List<Contribution> { new Contribution { Amount = 1000, DateMade = contributionDate } });
        _mockSnapshotRepository.Setup(r => r.GetLatestByAssetIdAsync(1))
            .ReturnsAsync(new Snapshot { TotalValue = 1100, SnapshotDate = snapshotDate });

        // FeeCalculator: 1000 * 0.01 * 365 / 365 = 10
        _mockFeeCalculator.Setup(f => f.CalculateFee(1000m, 0.01m, contributionDate, snapshotDate))
            .Returns(10m);

        // Act
        var summary = await _service.GetPortfolioSummaryAsync();

        // Assert
        summary.TotalInvested.Should().Be(1000m);
        summary.TotalValue.Should().Be(1100m);
        summary.TotalPnL.Should().Be(100m);
        summary.TotalFees.Should().Be(10m);
        summary.NetPnL.Should().Be(90m); // 100 - 10
        
        _mockFeeCalculator.Verify(f => f.CalculateFee(1000m, 0.01m, contributionDate, snapshotDate), Times.Once);
    }
}
