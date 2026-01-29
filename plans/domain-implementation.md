# Domain Logic Implementation Plan

## Overview
Implement the core business logic and data access layer for the Investment Tracker.

## Tasks

### 1. Repository Interfaces (Domain)
Create the following interfaces in `src/InvestmentTracker.Domain/Interfaces`:
- `IAssetRepository`: Methods to GetById, GetAll, Add, Update.
- `IContributionRepository`: Methods to GetByAssetId, Add.
- `ISnapshotRepository`: Methods to GetByAssetId, Add.

### 2. Repository Implementation (Infra)
Implement the interfaces in `src/InvestmentTracker.Infra/Repositories`:
- Use `InvestmentContext` (EF Core) to implement the data access.
- Ensure async/await usage.

### 3. FeeCalculator (Domain)
Implement `src/InvestmentTracker.Domain/Services/FeeCalculator.cs`.
- **Logic**: Calculate the fee for a period.
- **Formula**: `Fee = Principal * (AnnualRate / 365) * Days` (Simple interest approximation for daily fees).
- **Interface**: `decimal CalculateFee(decimal principal, decimal annualRate, DateTime startDate, DateTime endDate)`

### 4. PortfolioService (Domain)
Implement `src/InvestmentTracker.Domain/Services/PortfolioService.cs` and update `IPortfolioService.cs`.
- **Dependencies**: Inject Repositories and FeeCalculator.
- **Methods**:
    - `Task<Asset> CreateAssetAsync(Asset asset)`
    - `Task AddContributionAsync(int assetId, Contribution contribution)`
    - `Task AddSnapshotAsync(int assetId, Snapshot snapshot)`
    - `Task<PortfolioSummary> GetPortfolioSummaryAsync()`
        - This should iterate all assets.
        - Sum current values (from latest snapshot).
        - Calculate total invested (sum of contributions).
        - Calculate PnL.

### 5. Unit Tests
- Create tests in `tests/InvestmentTracker.Tests` for `FeeCalculator` and `PortfolioService`.
