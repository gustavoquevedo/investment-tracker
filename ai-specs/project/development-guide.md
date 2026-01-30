# Investment Tracker Development Guide

This guide provides instructions for setting up the development environment, building, and testing the Investment Tracker application.

## Prerequisites

Ensure you have the following installed:

- **.NET 10 SDK** (or the version defined in `global.json`)
- **Visual Studio Code** (with C# Dev Kit) or **Visual Studio 2022+**
- **Git**

## Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd investment-tracker
```

### 2. Build the Solution

```bash
dotnet restore
dotnet build
```

### 3. Database Setup

The application uses a local SQLite database (`investments.db`).

```bash
# Install EF Core tools globally if not installed
dotnet tool install --global dotnet-ef

# Update the database
dotnet ef database update --project src/InvestmentTracker.Infra --startup-project src/InvestmentTracker.UI
```

### 4. Run the Application

```bash
dotnet run --project src/InvestmentTracker.UI
```

### 5. Run Tests

```bash
# Run all tests
dotnet test

# Run as standalone executable (MTP)
./tests/InvestmentTracker.Tests/bin/Debug/net10.0/InvestmentTracker.Tests.exe
```

## Common Tasks

### Adding a New Asset Type

1. Update the `AssetType` enum in `InvestmentTracker.Domain`
2. Add logic in `FeeCalculator` or other services if necessary
3. Add a migration if the database schema is affected

### Adding a New Feature

1. Define the Entity or Service Interface in **Domain**
2. Implement the Interface in **Infra** (if it involves I/O)
3. Implement the Logic in **Domain Services** (if pure business logic)
4. Expose/Use the feature in **UI**
5. Write tests in **Tests**

### Adding a Migration

```bash
dotnet ef migrations add <MigrationName> --project src/InvestmentTracker.Infra --startup-project src/InvestmentTracker.UI
```

## Troubleshooting

- **EF Core Tools not found**: Ensure you have installed the tools (`dotnet tool install --global dotnet-ef`) and that your PATH is configured
- **Database locked**: Ensure no other process (like a DB viewer) has the `investments.db` file open
