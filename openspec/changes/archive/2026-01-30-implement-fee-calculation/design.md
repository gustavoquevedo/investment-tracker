## Context

The `FeeCalculator` service exists and passes unit tests, but it is not used in the portfolio summary calculation. The P&L shown to users is "Gross P&L" (before fees), which overstates real returns.

## Goals / Non-Goals

**Goals:**
- Integrate `FeeCalculator` into `GetPortfolioSummaryAsync()`
- Calculate fees based on period between snapshots
- Expose `TotalFees` and `NetPnL` in the `PortfolioSummary` DTO

**Non-Goals:**
- Complex fee schedules (tiered fees, performance fees)
- Per-asset fee breakdown (aggregate only for now)

## Decisions

### 1. Fee Calculation Period

**Decision:** Calculate fees from the first contribution date to the latest snapshot date for each asset.

**Rationale:** Simple and accurate for the "holding period". Uses existing dates from contributions and snapshots.

**Alternative Considered:** Calculate fees on a per-snapshot-interval basis.
**Rejected:** More complex, marginal improvement in accuracy.

### 2. Principal for Fee Calculation

**Decision:** Use the sum of contributions as the principal (money invested).

**Rationale:** Fees are charged on invested capital, not market value.

## Implementation Approach

1. In `GetPortfolioSummaryAsync()`:
   - For each asset, get earliest contribution date and latest snapshot date.
   - Use `_feeCalculator.CalculateFee(contributions.Sum, asset.FeePercentagePerYear, earliestContribution, latestSnapshot)`.
   - Sum all fees into `TotalFees`.
   - Set `NetPnL = TotalPnL - TotalFees`.

2. Update `PortfolioSummary` DTO:
   - Add `TotalFees` property.
   - Add `NetPnL` property.

## Verification Plan

### Automated Tests
- **Command**: `dotnet test` (runs all tests)
- **Existing Test**: `GetPortfolioSummaryAsync_ShouldCalculateTotalsCorrectly` - needs update to verify fees.
- **New Test**: Add test that mocks `_feeCalculator` to return a known fee, then verify `TotalFees` and `NetPnL`.

### Manual Verification
- N/A (unit tests are sufficient for this domain logic change)
