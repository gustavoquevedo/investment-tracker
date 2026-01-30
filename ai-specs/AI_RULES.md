# AI Development Rules (Index)

## Project Overview
Investment Tracker is a personal finance app for tracking assets (ETFs, Crypto, Cash) and their historical values.

**Stack**:
- **Backend**: .NET 10 Web API, EF Core (SQLite), C# 14
- **Frontend**: React 19, TypeScript, Vite, TailwindCSS
- **Tools**: .slnx, Central Package Management (CPM), Microsoft.Testing.Platform

## Development Standards
**IMPORTANT**: This project follows strict coding standards. You MUST read and adhere to the files linked below:

### Universal Rules (`ai-specs/base/`)
- `csharp-standards.md`: C# 14 types, naming, patterns (use `field`, `required`, file-scoped namespaces).
- `architecture.md`: Vertical Slice Architecture (VSA) for API, Clean Architecture for Domain.
- `dotnet-tooling.md`: Use `.slnx` and CPM in `.csproj` files.
- `testing-standards.md`: xUnit v3, MTP, FluentAssertions, Moq.
- `frontend-standards.md`: React functional components, hooks, React Query.
- `documentation.md`: English only, clear naming, incremental changes.

### Project Rules (`ai-specs/project/`)
- `architecture.md`: Folder mapping (src/InvestmentTracker.Domain, src/InvestmentTracker.Infra, etc.).
- `domain-model.md`: Definitions of `Asset`, `Snapshot`, `Contribution`.
- `api-spec.yml`: REST API contract.

## Build & Test Commands
- **Build**: `dotnet build`
- **Test**: `dotnet test` or `./tests/InvestmentTracker.Tests/bin/Debug/net10.0/InvestmentTracker.Tests.exe`
- **Start API**: `dotnet run --project src/InvestmentTracker.UI`
- **DB Update**: `dotnet ef database update --project src/InvestmentTracker.Infra --startup-project src/InvestmentTracker.UI`
- **Frontend**: `cd src/InvestmentTracker.Client && npm run dev`

## Commit Style
Conventional Commits: `feat(scope): verify change`
