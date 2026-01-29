## Why

Implement the core domain business logic and data access layer to enable tracking of asset values, contributions, and fee calculations. This provides the foundational backend services required for the Investment Tracker application.

## What Changes

- Implement Repository interfaces (`IAssetRepository`, `IContributionRepository`, `ISnapshotRepository`) in the Domain layer.
- Implement Repository classes (`AssetRepository`, `ContributionRepository`, `SnapshotRepository`) in the Infra layer using EF Core.
- Implement `FeeCalculator` service logic for daily fee calculations.
- Implement `PortfolioService` to manage assets, contributions, snapshots, and generate portfolio summaries.
- Add Unit Tests for `FeeCalculator` and `PortfolioService` to ensure correctness.

## Capabilities

### New Capabilities
- `fee-calculation`: Logic for calculating fees based on principal, annual rate, and duration.
- `portfolio-management`: Core services for creating and managing assets, contributions, and snapshots.
- `portfolio-reporting`: Capability to aggregate data and generate portfolio performance summaries (Total Invested, Total Value, PnL).

### Modified Capabilities
<!-- No existing capabilities are being modified as this is the first iteration. -->

## Impact

- **Domain Layer**:
  - New Interfaces: `IAssetRepository`, `IContributionRepository`, `ISnapshotRepository`
  - New Services: `FeeCalculator`, `PortfolioService` (implementation)
  - Updated: `IPortfolioService`
- **Infrastructure Layer**:
  - New Repositories: `AssetRepository`, `ContributionRepository`, `SnapshotRepository`
- **Tests**:
  - New test classes in `tests/InvestmentTracker.Tests/` for `FeeCalculator` and `PortfolioService`.
