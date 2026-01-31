# Tasks: VSA Refactor

## 1. Setup

- [x] 1.1 Create `Features/` folder structure in `InvestmentTracker.Api` with subfolders: `Assets/`, `Portfolio/`, `Tags/`, `Common/`
- [x] 1.2 Create `Common/TagDto.cs` for the shared tag DTO used across features
- [x] 1.3 Create endpoint registration extension method stubs: `AssetsEndpoints.cs`, `PortfolioEndpoints.cs`, `TagsEndpoints.cs`

## 2. Migrate Portfolio Endpoints

- [x] 2.1 Create `Features/Portfolio/GetSummary.cs` with nested DTOs and handler logic (move from `PortfolioController.GetSummary` + `PortfolioService.GetPortfolioSummaryAsync`)
- [x] 2.2 Create `Features/Portfolio/GetHistory.cs` with handler logic (move from `PortfolioController.GetHistory` + `PortfolioService.GetHistoryAsync`)
- [x] 2.3 Create `Features/Portfolio/GetAllocation.cs` with handler logic (move from `PortfolioController.GetAllocation` + `PortfolioService.GetAllocationAsync`)
- [x] 2.4 Create `Features/Portfolio/GetReturns.cs` with handler logic (move from `PortfolioController.GetReturns` + `PortfolioService.GetReturnsAsync`)
- [x] 2.5 Register all portfolio endpoints in `PortfolioEndpoints.cs`
- [x] 2.6 Delete `Controllers/PortfolioController.cs`
- [x] 2.7 Delete `DTOs/PortfolioDTOs.cs`

## 3. Migrate Asset Endpoints

- [x] 3.1 Create `Features/Assets/GetAssets.cs` (GET /assets with tag filter)
- [x] 3.2 Create `Features/Assets/GetAssetById.cs` (GET /assets/{id})
- [x] 3.3 Create `Features/Assets/CreateAsset.cs` (POST /assets)
- [x] 3.4 Create `Features/Assets/UpdateAsset.cs` (PUT /assets/{id})
- [x] 3.5 Create `Features/Assets/DeleteAsset.cs` (DELETE /assets/{id})
- [x] 3.6 Create `Features/Assets/GetSnapshots.cs` (GET /assets/{id}/snapshots)
- [x] 3.7 Create `Features/Assets/AddSnapshot.cs` (POST /assets/{id}/snapshots)
- [x] 3.8 Create `Features/Assets/GetContributions.cs` (GET /assets/{id}/contributions)
- [x] 3.9 Create `Features/Assets/AddContribution.cs` (POST /assets/{id}/contributions)
- [x] 3.10 Register all asset endpoints in `AssetsEndpoints.cs`
- [x] 3.11 Delete `Controllers/AssetsController.cs`
- [x] 3.12 Delete `DTOs/AssetDTOs.cs`

## 4. Migrate Tag Endpoints

- [x] 4.1 Create `Features/Tags/GetTags.cs` (GET /tags)
- [x] 4.2 Create `Features/Tags/CreateTag.cs` (POST /tags)
- [x] 4.3 Create `Features/Tags/DeleteTag.cs` (DELETE /tags/{id})
- [x] 4.4 Register all tag endpoints in `TagsEndpoints.cs`
- [x] 4.5 Delete `Controllers/TagsController.cs`
- [x] 4.6 Delete `DTOs/TagDTOs.cs`

## 5. Update Program.cs

- [x] 5.1 Remove `AddControllers()` and `MapControllers()` calls
- [x] 5.2 Add `app.MapAssetEndpoints()`, `app.MapPortfolioEndpoints()`, `app.MapTagEndpoints()`
- [x] 5.3 Remove repository DI registrations (IAssetRepository, ISnapshotRepository, IContributionRepository, ITagRepository)
- [x] 5.4 Keep domain service registrations (IFeeCalculator)

## 6. Clean Up Domain Layer

- [x] 6.1 Extract `ReturnCalculator` from `PortfolioService` as a separate domain service (for TWR/MWR calculations)
- [x] 6.2 Delete `PortfolioService.cs` (logic now in slices)
- [x] 6.3 Delete `IPortfolioService.cs` interface
- [x] 6.4 Delete repository interfaces from Domain: `IAssetRepository.cs`, `ISnapshotRepository.cs`, `IContributionRepository.cs`, `ITagRepository.cs`

## 7. Clean Up Infrastructure Layer

- [x] 7.1 Delete repository implementations: `AssetRepository.cs`, `SnapshotRepository.cs`, `ContributionRepository.cs`, `TagRepository.cs`
- [x] 7.2 Delete `Repositories/` folder if empty

## 8. Update Integration Tests

- [x] 8.1 Update test files to work with Minimal API endpoints (verify existing tests still pass)
- [x] 8.2 Run full test suite: `dotnet test`

## 9. Update Project Specs

- [x] 9.1 Update `ai-specs/project/architecture.md` to reflect VSA structure and remove N-Tier references
- [x] 9.2 Add README.md in `Features/` folder explaining the VSA pattern

## 10. Verification

- [x] 10.1 Build the solution: `dotnet build`
- [x] 10.2 Run all tests: `dotnet test`
- [x] 10.3 Start API and verify Swagger shows all endpoints correctly
- [x] 10.4 Test one endpoint from each feature group manually via Swagger or curl
