using Microsoft.AspNetCore.Mvc;
using InvestmentTracker.Domain.Interfaces;
using InvestmentTracker.Api.DTOs;

namespace InvestmentTracker.Api.Controllers;

[ApiController]
[Route("portfolio")]
public class PortfolioController : ControllerBase
{
    private readonly IPortfolioService _portfolioService;

    public PortfolioController(IPortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }

    /// <summary>
    /// Get portfolio summary with aggregate metrics.
    /// </summary>
    [HttpGet("summary")]
    public async Task<ActionResult<PortfolioSummaryDto>> GetSummary()
    {
        var summary = await _portfolioService.GetPortfolioSummaryAsync();

        return Ok(new PortfolioSummaryDto(
            summary.TotalValue,
            summary.TotalInvested,
            summary.TotalPnL,
            summary.TotalInvested > 0 
                ? Math.Round(summary.TotalPnL / summary.TotalInvested * 100, 2) 
                : 0,
            summary.AssetCount,
            summary.LastUpdated));
    }

    /// <summary>
    /// Get portfolio value history for charting.
    /// </summary>
    [HttpGet("history")]
    public async Task<ActionResult<PortfolioHistoryDto>> GetHistory(
        [FromQuery] DateOnly? from = null,
        [FromQuery] DateOnly? to = null)
    {
        var history = await _portfolioService.GetHistoryAsync(from, to);

        var points = history.Points.Select(p => 
            new PortfolioHistoryPointDto(p.Date, p.Value, p.Invested)).ToList();

        return Ok(new PortfolioHistoryDto(points));
    }

    /// <summary>
    /// Get asset allocation breakdown by type.
    /// </summary>
    [HttpGet("allocation")]
    public async Task<ActionResult<PortfolioAllocationDto>> GetAllocation()
    {
        var allocation = await _portfolioService.GetAllocationAsync();

        var entries = allocation.ByType.Select(e => 
            new AllocationEntryDto(e.Type, e.Value, e.Percentage)).ToList();

        return Ok(new PortfolioAllocationDto(entries));
    }

    /// <summary>
    /// Get calculated portfolio returns.
    /// </summary>
    [HttpGet("returns")]
    public async Task<ActionResult<PortfolioReturnsDto>> GetReturns(
        [FromQuery] DateOnly? asOf = null)
    {
        var returns = await _portfolioService.GetReturnsAsync(asOf);

        return Ok(new PortfolioReturnsDto(
            returns.Twr,
            returns.Mwr,
            new PeriodReturnsDto(
                returns.Periods.Ytd,
                returns.Periods.OneYear,
                returns.Periods.ThreeYear,
                returns.Periods.FiveYear,
                returns.Periods.AllTime)));
    }
}
