# Design: VSA Refactor

## Context

The Investment Tracker API currently uses a traditional N-Tier architecture:
- **3 Controllers** (`AssetsController`, `PortfolioController`, `TagsController`) containing ~16 endpoints
- **Shared DTOs** in a global `DTOs/` folder
- **Centralized Services** (`PortfolioService` at 250+ lines) handling all business logic
- **Repository Pattern** with 4 repository interfaces/implementations

This design document describes how to refactor to Vertical Slice Architecture (VSA) as specified in the base architecture standards.

## Goals / Non-Goals

**Goals:**
- Organize API project by **feature** rather than technical layer
- Each endpoint becomes a self-contained **slice** with its own DTOs and handler
- Reduce coupling between unrelated features
- Align project structure with base specs (`ai-specs/base/architecture.md`)
- Update project-specific specs to prevent future drift

**Non-Goals:**
- Changing the Domain layer structure (Entities stay shared)
- Modifying database schema or migrations
- Adding new features or endpoints
- Changing the React frontend

## Decisions

### 1. Folder Structure: Feature-First Organization

**Decision**: Organize by business capability, then by action.

```
src/InvestmentTracker.Api/
├── Features/
│   ├── Assets/
│   │   ├── GetAssets.cs           # GET /assets
│   │   ├── GetAssetById.cs        # GET /assets/{id}
│   │   ├── CreateAsset.cs         # POST /assets
│   │   ├── UpdateAsset.cs         # PUT /assets/{id}
│   │   ├── DeleteAsset.cs         # DELETE /assets/{id}
│   │   ├── GetSnapshots.cs        # GET /assets/{id}/snapshots
│   │   ├── AddSnapshot.cs         # POST /assets/{id}/snapshots
│   │   ├── GetContributions.cs    # GET /assets/{id}/contributions
│   │   └── AddContribution.cs     # POST /assets/{id}/contributions
│   ├── Portfolio/
│   │   ├── GetSummary.cs          # GET /portfolio/summary
│   │   ├── GetHistory.cs          # GET /portfolio/history
│   │   ├── GetAllocation.cs       # GET /portfolio/allocation
│   │   └── GetReturns.cs          # GET /portfolio/returns
│   └── Tags/
│       ├── GetTags.cs             # GET /tags
│       ├── CreateTag.cs           # POST /tags
│       └── DeleteTag.cs           # DELETE /tags/{id}
├── Common/                         # Shared utilities (if any)
│   └── TagDto.cs                  # Truly shared DTO (used in Asset responses)
└── Program.cs                      # Minimal API registrations
```

**Rationale**: One file per endpoint keeps related code together and makes feature discovery trivial.

### 2. Endpoint Implementation: Static Classes with Minimal API

**Decision**: Use static classes with `Map` extension methods.

```csharp
// Features/Portfolio/GetSummary.cs
namespace InvestmentTracker.Api.Features.Portfolio;

public static class GetSummary
{
    public record Response(
        decimal TotalValue,
        decimal TotalInvested,
        decimal PnL,
        decimal PnLPercent,
        int AssetCount,
        DateTime? LastUpdated);

    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/portfolio/summary", Handle)
           .WithName("GetPortfolioSummary")
           .WithTags("Portfolio");
    }

    private static async Task<IResult> Handle(
        InvestmentContext db,
        IFeeCalculator feeCalculator)
    {
        // Logic directly here or delegated to a handler method
    }
}
```

**Alternatives Considered**:
- **FastEndpoints library**: More structure but adds a dependency. Rejected for simplicity.
- **MediatR handlers**: Good for CQRS but overkill for this project size.
- **Keep Controllers**: Would defeat the purpose of VSA.

**Rationale**: Minimal API is built-in, lightweight, and aligns with modern .NET patterns.

### 3. DTO Strategy: Co-located with Duplication Allowed

**Decision**: DTOs live inside their feature file as nested records.

```csharp
public static class CreateAsset
{
    public record Request(string Name, string AssetType, string? ISIN, string? Ticker, decimal FeePercentagePerYear);
    public record Response(int Id, string Name, string AssetType, ...);
    // ...
}
```

**Exception**: `TagDto` is used in both Asset responses and Tag responses. It moves to `Common/TagDto.cs`.

**Rationale**: Co-location maximizes cohesion. Duplication is explicitly preferred over premature abstraction (per base specs: "Duplication is cheaper than the wrong abstraction").

### 4. Data Access: Direct DbContext Injection (Retire Repositories)

**Decision**: Inject `InvestmentContext` directly into handlers. Retire repository interfaces for simple CRUD.

**Current**:
```csharp
// 4 repository interfaces + 4 implementations = 8 files
services.AddScoped<IAssetRepository, AssetRepository>();
```

**New**:
```csharp
// Direct context injection
Handle(InvestmentContext db, ...) => db.Assets.Where(...);
```

**What to Keep**:
- `IFeeCalculator` / `FeeCalculator`: Pure domain logic, no data access
- Complex query logic: Extract to `IQueryable<T>` extension methods in Domain if reused

**Rationale**: Repositories add indirection without value for simple CRUD. The `DbContext` is already a Unit of Work + Repository.

### 5. Domain Services: Extract Calculation Logic

**Decision**: Keep pure calculation logic in Domain; move orchestration to slices.

| Current Location | New Location |
|------------------|--------------|
| `PortfolioService.GetPortfolioSummaryAsync()` | `Features/Portfolio/GetSummary.Handle()` |
| `PortfolioService.CreateAssetAsync()` | `Features/Assets/CreateAsset.Handle()` |
| `FeeCalculator.CalculateFee()` | **Keep** in `Domain/Services/FeeCalculator.cs` |
| Return calculation logic | **Keep** in `Domain/Services/ReturnCalculator.cs` (extract from PortfolioService) |

**Rationale**: Orchestration (load data → calculate → return) belongs in slices. Pure math (fee calculation, TWR/MWR) stays in reusable domain services.

### 6. Registration: Extension Method per Feature Group

**Decision**: Each feature area provides a `MapXxxEndpoints` extension method.

```csharp
// Program.cs
app.MapAssetEndpoints();
app.MapPortfolioEndpoints();
app.MapTagEndpoints();

// Features/Assets/AssetsEndpoints.cs
public static class AssetsEndpoints
{
    public static void MapAssetEndpoints(this IEndpointRouteBuilder app)
    {
        GetAssets.Map(app);
        GetAssetById.Map(app);
        CreateAsset.Map(app);
        // ...
    }
}
```

**Rationale**: Keeps `Program.cs` clean while maintaining discoverability.

## Migration Plan

### Phase 1: Setup (15 min)
1. Create `Features/` folder structure
2. Create `Common/` folder with shared `TagDto`
3. Add endpoint registration extension methods

### Phase 2: Migrate Portfolio Endpoints (30 min)
1. Create `Features/Portfolio/GetSummary.cs` (move logic from PortfolioController + PortfolioService)
2. Create `Features/Portfolio/GetHistory.cs`
3. Create `Features/Portfolio/GetAllocation.cs`
4. Create `Features/Portfolio/GetReturns.cs`
5. Delete `PortfolioController.cs`

### Phase 3: Migrate Asset Endpoints (45 min)
1. Create all 9 asset slices
2. Move DTO definitions inline
3. Delete `AssetsController.cs`

### Phase 4: Migrate Tag Endpoints (15 min)
1. Create 3 tag slices
2. Delete `TagsController.cs`

### Phase 5: Cleanup (15 min)
1. Delete `DTOs/` folder
2. Delete `Controllers/` folder
3. Remove repository interfaces and implementations
4. Update `Program.cs` DI registrations
5. Extract `ReturnCalculator` from `PortfolioService`
6. Delete `PortfolioService` (now empty)

### Phase 6: Spec Updates (10 min)
1. Update `ai-specs/project/architecture.md` to reflect VSA structure
2. Ensure alignment with base specs

## Risks / Trade-offs

| Risk | Impact | Mitigation |
|------|--------|------------|
| **Integration tests break** | High | Tests reference old controller names. Update test files after migration. |
| **Unfamiliar pattern for contributors** | Medium | Add README in Features/ explaining the pattern. |
| **Losing IntelliSense grouping** | Low | One file per endpoint is more discoverable than scrolling through 200-line controllers. |
| **Swagger documentation** | Low | Minimal API supports `WithTags()`, `WithName()`, and XML comments for OpenAPI. |

## Open Questions

1. **Should we adopt FastEndpoints later?** - For now, raw Minimal API is sufficient. Revisit if we add 50+ endpoints.
2. **Should Domain DTOs (e.g., `PortfolioSummary`) be removed?** - Keep for now; they represent domain concepts returned from domain services.
