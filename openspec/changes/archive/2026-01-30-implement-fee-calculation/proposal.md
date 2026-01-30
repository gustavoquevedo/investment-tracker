## Why

The `FeeCalculator` service exists and is tested, but it is not integrated into the portfolio P&L calculation. Management fees are silently eroding returns, but users cannot see this impact. Integrating fee calculation into `GetPortfolioSummaryAsync` will provide accurate Net P&L (after fees).

## What Changes

- **Integrate Fee Calculation into Portfolio Summary**: Modify `PortfolioService.GetPortfolioSummaryAsync()` to calculate cumulative fees for each asset and include them in the summary.
- **Add `TotalFees` property to `PortfolioSummary` DTO**: Expose the total fees calculated across all assets.
- **Add `NetPnL` property**: `TotalPnL - TotalFees` gives net return after fees.
- **Unit Tests**: Add tests for the new integration.

## Capabilities

### New Capabilities
None (the `fee-calculation` capability already exists in specs).

### Modified Capabilities
- `portfolio-management`: Add `TotalFees` and `NetPnL` to summary output.

## Impact

- **Code**: `PortfolioService.cs`, `PortfolioSummary.cs`
- **Tests**: `PortfolioServiceTests.cs`
- **API**: `GET /portfolio/summary` response will include `totalFees` and `netPnL`.
