namespace InvestmentTracker.Api.DTOs;

/// <summary>
/// Portfolio summary with aggregate metrics.
/// </summary>
public record PortfolioSummaryDto(
    decimal TotalValue,
    decimal TotalInvested,
    decimal PnL,
    decimal PnLPercent,
    int AssetCount,
    DateTime? LastUpdated);

/// <summary>
/// Historical portfolio values for charting.
/// </summary>
public record PortfolioHistoryDto(List<PortfolioHistoryPointDto> Points);

public record PortfolioHistoryPointDto(
    DateOnly Date,
    decimal Value,
    decimal Invested);

/// <summary>
/// Asset allocation breakdown by type.
/// </summary>
public record PortfolioAllocationDto(List<AllocationEntryDto> ByType);

public record AllocationEntryDto(
    string Type,
    decimal Value,
    decimal Percentage);

/// <summary>
/// Calculated portfolio returns.
/// </summary>
public record PortfolioReturnsDto(
    decimal Twr,
    decimal Mwr,
    PeriodReturnsDto Periods);

public record PeriodReturnsDto(
    decimal Ytd,
    decimal OneYear,
    decimal ThreeYear,
    decimal FiveYear,
    decimal AllTime);
