---
description: Investment Tracker project-specific architecture, layer mapping, and folder structure.
globs: ["src/**/*.cs", "tests/**/*.cs"]
alwaysApply: true
---

# Investment Tracker Architecture

## Project Structure

```text
src/
  InvestmentTracker.Api/      # Features (Vertical Slices), Entry Point
  InvestmentTracker.Domain/   # Entities, Interfaces, Pure Logic Services
  InvestmentTracker.Infra/    # EF Core DbContext, Data Migrations
tests/
  InvestmentTracker.Api.Tests # Integration Tests
  InvestmentTracker.Domain.Tests # Unit Tests
```

## Layer Mapping

This project follows **Vertical Slice Architecture (VSA)** for the API layer and **Clean Architecture** principles for core domain logic.

### API Layer (`src/InvestmentTracker.Api`)

Organized by **Feature Slices** using the **subdirectory pattern** in `Features/` folder.

**Structure:**
```text
Features/
├── Assets/
│   ├── GetAssets/
│   │   ├── GetAssetsEndpoint.cs
│   │   └── GetAssetsDtos.cs
│   ├── CreateAsset/ ...
│   ├── ManageSnapshots/ ...
│   └── AssetsEndpoints.cs
├── Portfolio/
│   ├── GetSummary/
│   │   ├── GetSummaryEndpoint.cs
│   │   └── SummaryDtos.cs
│   └── PortfolioEndpoints.cs
├── Tags/ ...
└── Common/
    └── TagDto.cs
```

**Key Principles:**
- Each endpoint is in its **own subdirectory** (e.g., `GetAssets/`)
- Separate `<Action>Endpoint.cs` and `<Action>Dtos.cs` files per slice
- **Data Access**: Slices inject `InvestmentContext` directly (no repositories for simple CRUD)
- **Registration**: Extension methods per feature group in `<Group>Endpoints.cs`

**Dependencies**: Domain, Infrastructure

### Domain Layer (`src/InvestmentTracker.Domain`)

Contains pure business logic and core concepts:
- **Entities**: `Asset`, `Snapshot`, `Contribution`, `Tag`, `AssetTag`
- **Interfaces**: Strategy interfaces (e.g., `IFeeCalculator`, `IReturnCalculator`)
- **Domain Services**: Pure logic implementations (e.g., `FeeCalculator`, `ReturnCalculator`)
- **Enums**: `AssetType`

**Dependencies**: None (Pure C#)

### Infrastructure Layer (`src/InvestmentTracker.Infra`)

Focuses on persistence:
- **DbContext**: `InvestmentContext`
- **Entity Configurations**: Fluent API `IEntityTypeConfiguration<T>` implementations
- **Migrations**: EF Core migrations

**Dependencies**: Domain, Entity Framework Core

## Database

- **Local Development**: SQLite (`InvestmentTracker.db`)
- **Production**: PostgreSQL

## EF Core Commands

```bash
# Add migration
dotnet ef migrations add <Name> --project src/InvestmentTracker.Infra --startup-project src/InvestmentTracker.Api

# Update database
dotnet ef database update --project src/InvestmentTracker.Infra --startup-project src/InvestmentTracker.Api
```
