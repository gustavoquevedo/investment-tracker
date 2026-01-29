## Context

The Investment Tracker application currently has basic domain entities and an EF Core `DbContext`, but lacks the core business logic and data access abstraction layers required to perform operations like adding assets, recording contributions, and calculating fees. This design addresses the implementation of the Domain and Infrastructure layers to support these capabilities.

## Goals / Non-Goals

**Goals:**
- Implement a robust Repository pattern for data access (`AssetRepository`, `ContributionRepository`, `SnapshotRepository`).
- Implement the `FeeCalculator` domain service with specific business logic for daily fee computation.
- Implement the `PortfolioService` to orchestrate domain operations and provide portfolio-level reporting.
- Ensure all domain logic is covered by unit tests.

**Non-Goals:**
- Implementation of API controllers or endpoints (this will be handled in a separate change/layer).
- Frontend integration.
- Database migration execution (assumed to be handled separately or via EF tools).

## Decisions

### Repository Pattern
We will define specific repository interfaces (e.g., `IAssetRepository`) rather than a generic `IRepository<T>` to allow for domain-specific query methods (e.g., `GetByAssetIdAsync`).
- **Rationale**: Provides better type safety and explicit contracts for data access needs.
- **Implementation**: Implemented in `InvestmentTracker.Infra` using `InvestmentContext`.

### Fee Calculation Logic
The fee calculation will use a simple interest approximation: `Principal * (AnnualRate / 365) * Days`.
- **Rationale**: Simplifies calculation for daily tracking.
- **Trade-off**: Slightly less precise than compound interest but sufficient for the current requirements.

### Portfolio Summary Aggregation
`GetPortfolioSummaryAsync` will aggregate data in-memory after fetching necessary entities/snapshots.
- **Rationale**: Keeps the logic within the Domain service.
- **Alternative**: SQL aggregation. chosen in-memory for now for simplicity given the expected data volume for MVP.

### Dependency Injection
Services and Repositories will be registered in the DI container. `PortfolioService` will depend on `IAssetRepository`, `IContributionRepository`, `ISnapshotRepository`, and `FeeCalculator`.

## Risks / Trade-offs

### Performance of Portfolio Summary
- **Risk**: `GetPortfolioSummaryAsync` iterates over all assets and fetches latest snapshots. This could be slow with a large number of assets.
- **Mitigation**: For the initial iteration (MVP), this is acceptable. Future optimization could involve caching or database-side aggregation (Views/Stored Procs).

### Precision
- **Risk**: Financial calculations with `decimal` might have rounding discrepancies if not standardized.
- **Mitigation**: Use `decimal` for all monetary values and defined rounding rules where necessary.
