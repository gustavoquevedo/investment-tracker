## 1. Project Setup

- [x] 1.1 Create `InvestmentTracker.Api` ASP.NET Core Web API project.
- [x] 1.2 Add project references to `InvestmentTracker.Domain` and `InvestmentTracker.Infra`.
- [x] 1.3 Install necessary NuGet packages (Microsoft.EntityFrameworkCore.Sqlite, Swashbuckle.AspNetCore).

## 2. Configuration

- [x] 2.1 Configure `Program.cs` to register `InvestmentContext` with SQLite connection string.
- [x] 2.2 Register Repositories (`AssetRepository`, etc.) and Services (`PortfolioService`) in DI container.
- [x] 2.3 Configure Swagger/OpenAPI generation to match `api-spec.yml`.

## 3. Implementation

- [x] 3.1 Implement `AssetsController` base CRUD operations (List, Create).
- [x] 3.2 Implement `AssetsController` item operations (Get, Update, Delete).
- [x] 3.3 Implement `AssetsController` sub-resources (Snapshots endpoints).
- [x] 3.4 Implement `AssetsController` sub-resources (Contributions endpoints).
- [x] 3.5 Implement `TagsController` (List, Create, Delete).

## 4. Verification

- [x] 4.1 Launch API and verify Swagger UI works.
- [x] 4.2 Verify endpoints connect to the database and return data.
