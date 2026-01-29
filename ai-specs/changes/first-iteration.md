## [Original]

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

## [Enhanced]

# Domain Logic Implementation Specification

## User Story
**As a** Backend Developer,
**I want to** implement the core domain services and repositories,
**So that** the application can correctly track asset values, process contributions, and calculate fees according to the business rules.

## Acceptance Criteria

### 1. Fee Calculation
- [ ] `FeeCalculator` must implement `CalculateFee(principal, annualRate, startDate, endDate)`.
- [ ] The calculation must use the formula: `Principal * (AnnualRate / 365.0) * Days`.
- [ ] If `startDate` > `endDate`, throw `ArgumentException`.
- [ ] `FeePercentagePerYear` (e.g., 0.005 for 0.5%) must be handled correctly.

### 2. Asset Management (`PortfolioService`)
- [ ] `CreateAssetAsync` persists a new Asset and returns it with the generated ID.
- [ ] `CreateAssetAsync` validates that `Name` is not empty.

### 3. Contribution Management
- [ ] `AddContributionAsync` verifies the Asset exists (throws `KeyNotFoundException` if not).
- [ ] Persists the contribution linked to the correct Asset.

### 4. Snapshot Management
- [ ] `AddSnapshotAsync` verifies the Asset exists.
- [ ] Persists the snapshot.

### 5. Portfolio Reporting
- [ ] `GetPortfolioSummaryAsync` returns a DTO containing:
    - `TotalInvested` (Sum of all contributions).
    - `TotalValue` (Sum of latest snapshot values for all assets).
    - `TotalPnL` (TotalValue - TotalInvested).

## Technical Implementation Details

### Files to Create/Modify

1.  **`src/InvestmentTracker.Domain/Interfaces/`**
    -   `IAssetRepository.cs`
    -   `IContributionRepository.cs`
    -   `ISnapshotRepository.cs`
    -   `IPortfolioService.cs` (Update)

2.  **`src/InvestmentTracker.Infra/Repositories/`**
    -   `AssetRepository.cs` (Implements `IAssetRepository`)
    -   `ContributionRepository.cs`
    -   `SnapshotRepository.cs`

3.  **`src/InvestmentTracker.Domain/Services/`**
    -   `FeeCalculator.cs`
    -   `PortfolioService.cs`

### Interface Definitions

```csharp
// IAssetRepository
Task<Asset?> GetByIdAsync(int id);
Task<IEnumerable<Asset>> GetAllAsync();
Task<Asset> AddAsync(Asset asset);

// IContributionRepository
Task<IEnumerable<Contribution>> GetByAssetIdAsync(int assetId);
Task AddAsync(Contribution contribution);

// ISnapshotRepository
Task<Snapshot?> GetLatestByAssetIdAsync(int assetId);
Task AddAsync(Snapshot snapshot);
```

### Testing Strategy (xUnit)
-   **FeeCalculatorTests**:
    -   Test with 1 year duration (should equal full rate * principal).
    -   Test with 0 days (should be 0).
-   **PortfolioServiceTests** (Mock Repositories):
    -   Test `GetPortfolioSummary` sums correctly.
    -   Test `AddContribution` throws if asset missing.
