---
description: Investment Tracker project-specific architecture, layer mapping, and folder structure.
globs: ["src/**/*.cs", "tests/**/*.cs"]
alwaysApply: true
---

# Investment Tracker Architecture

## Project Structure

```text
src/
  InvestmentTracker.Domain/   # Entities, Interfaces, Domain Services
  InvestmentTracker.Infra/    # EF Core DbContext, Repositories Implementation
  InvestmentTracker.UI/       # Console Application / API Entry Point
tests/
  InvestmentTracker.Tests/    # Unit and Integration Tests
```

## Layer Mapping

This project uses the Clean Architecture + VSA hybrid described in the base architecture standards.

### Domain Layer (`src/InvestmentTracker.Domain`)

Contains:
- **Entities**: `Asset`, `Snapshot`, `Contribution`, `Tag`, `AssetTag`
- **Repository Interfaces**: `IAssetRepository`, `ISnapshotRepository`, etc.
- **Domain Services**: `FeeCalculator`, `PortfolioService`
- **Enums**: `AssetType`

**Dependencies**: None (Pure C#)

### Infrastructure Layer (`src/InvestmentTracker.Infra`)

Contains:
- **DbContext**: `InvestmentTrackerDbContext`
- **Repository Implementations**: `AssetRepository`, etc.
- **Entity Configurations**: `IEntityTypeConfiguration<T>` implementations
- **Migrations**: EF Core migrations

**Dependencies**: Domain, Entity Framework Core

### UI Layer (`src/InvestmentTracker.UI`)

Contains:
- Application entry point (Console or API)
- Dependency injection configuration
- Startup logic

**Dependencies**: Domain, Infrastructure

## Database

- **Local Development**: SQLite (`investments.db`)
- **Production**: PostgreSQL

## EF Core Commands

```bash
# Add migration
dotnet ef migrations add <Name> --project src/InvestmentTracker.Infra --startup-project src/InvestmentTracker.UI

# Update database
dotnet ef database update --project src/InvestmentTracker.Infra --startup-project src/InvestmentTracker.UI
```

## Test Project (`tests/InvestmentTracker.Tests`)

Organized by layer:
- `Domain/` - Entity and domain service tests
- `Services/` - Service layer tests
- `Infra/` - Repository and integration tests
