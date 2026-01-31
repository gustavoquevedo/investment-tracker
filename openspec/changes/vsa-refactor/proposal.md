# Proposal: VSA Refactor

## Motivation

The project's base architecture specs (`ai-specs/base/architecture.md`) mandate **Vertical Slice Architecture (VSA)** for the API layer. However, the current implementation follows a traditional **N-Tier / Layered** architecture, which contradicts this requirement. This discrepancy occurred because the project-specific specs (`ai-specs/project/architecture.md`) inadvertently reinforced the layered approach without explicitly adopting VSA.

**Problems with the current approach:**
1. **God Service Anti-Pattern**: `PortfolioService.cs` handles everything from simple CRUD to complex return calculations (250+ lines and growing).
2. **Repository Bloat**: Each new reporting feature requires new specialized methods in repository interfaces.
3. **High Coupling**: DTOs are shared between unrelated features, causing ripple effects when requirements change.
4. **Developer Friction**: Adding a single field requires touching 4-5 files across layers.

**Why now?**
The project is still small enough that a refactor is low-risk and high-value. Adding more reporting features (Dividend Tracking, Rebalancing, Sector Allocation) on the current architecture will accelerate the "maintenance wall."

## Change Description

Refactor the API layer from N-Tier to Vertical Slice Architecture:

1. **Restructure API Project**: Replace `Controllers/` and `DTOs/` folders with a `Features/` hierarchy organized by business capability.
2. **Migrate Endpoints**: Convert 3 Controllers (~16 endpoints) into individual feature slices.
3. **Relocate DTOs**: Move DTOs into their respective feature slices (embrace duplication over wrong abstractions).
4. **Slim Down Domain Services**: Extract reporting logic from `PortfolioService` into feature-specific handlers.
5. **Update Project Specs**: Align `ai-specs/project/architecture.md` with the base specs to prevent future drift.

## Capabilities

### New Capabilities
- `vsa-api-structure`: Feature-based folder organization in the API project (`Features/<Capability>/<Action>/`)
- `feature-specific-dtos`: DTOs co-located with their feature slices (no shared DTO folder)

### Modified Capabilities
- `portfolio-reporting`: Logic moves from centralized `PortfolioService` to individual feature handlers
- `asset-management`: CRUD operations become individual slices instead of a single controller

## Impact

### Code Changes
- **API Project**: Major restructure from 3 controllers to ~16 feature slice folders
- **Domain Project**: `PortfolioService` significantly reduced; pure calculation logic remains in domain services
- **Infrastructure Project**: Repository implementations may be retired or simplified

### Affected Systems
- API routing (will use Minimal API pattern or FastEndpoints)
- Dependency injection configuration in `Program.cs`
- Integration tests (need to update controller references)

### Dependencies
- No new package dependencies required
- Optional: Consider `FastEndpoints` NuGet package for structured endpoint definition

### Risk Assessment
- **Low Risk**: Core business logic is unchanged; this is purely organizational
- **Medium Effort**: ~2 hours of refactoring work
- **High Payoff**: Immediate improvement in developer velocity for future features
