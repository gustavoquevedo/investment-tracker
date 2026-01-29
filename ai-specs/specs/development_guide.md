# Development Guide

This guide provides instructions for setting up the development environment, building, and testing the Investment Tracker application.

## 🚀 Setup Instructions

### Prerequisites

Ensure you have the following installed:
-   **.NET 10 SDK** (or the specific version defined in `global.json` if present)
-   **Visual Studio Code** (with C# Dev Kit) or **Visual Studio 2022+**
-   **Git**

### 1. Clone the Repository

```bash
git clone <repository-url>
cd investment-tracker
```

### 2. Project Structure

The solution `InvestmentTracker.slnx` consists of the following projects:

-   **src/InvestmentTracker.Domain**: Core business logic, entities, and interfaces (Class Library).
-   **src/InvestmentTracker.Infra**: Data access, EF Core implementation, and infrastructure concerns (Class Library).
-   **src/InvestmentTracker.UI**: The application entry point (Console Application).
-   **tests/InvestmentTracker.Tests**: Unit and integration tests (xUnit).

### 3. Build the Solution

Restore dependencies and build the entire solution:

```bash
dotnet restore
dotnet build
```

### 4. Modernization Commands (.NET 10)

For existing components, use these commands to align with the latest project standards:

```bash
# Migrate legacy .sln to modern .slnx
dotnet sln migrate

# Central Package Management (CPM) is enabled via Directory.Packages.props
# and Directory.Build.props at the solution root.
```

### 5. Database Setup

The application uses a local SQLite database (`investments.db`) located in the root or UI directory.

To apply migrations or update the database:

```bash
# Install EF Core tools globally if not installed
dotnet tool install --global dotnet-ef

# Update the database
dotnet ef database update --project src/InvestmentTracker.Infra --startup-project src/InvestmentTracker.UI
```

To add a new migration after changing the Domain/Infra models:

```bash
dotnet ef migrations add <MigrationName> --project src/InvestmentTracker.Infra --startup-project src/InvestmentTracker.UI
```

### 5. Running the Application

To run the console application:

```bash
dotnet run --project src/InvestmentTracker.UI
```

### 6. Testing

Tests leverage **Microsoft.Testing.Platform (MTP)** for high-performance, standalone execution.

```bash
# Run all tests via the standard CLI
dotnet test

# Run specific test project as a standalone executable
./tests/InvestmentTracker.Tests/bin/Debug/net10.0/InvestmentTracker.Tests.exe
```

## 📦 Managing Dependencies

This project uses **Central Package Management (CPM)**. All NuGet package versions are defined in the `Directory.Packages.props` file at the root.

To add or update a dependency:
1.  Open `Directory.Packages.props`.
2.  Add or update the `<PackageVersion Include="PackageName" Version="x.y.z" />` entry.
3.  In the specific `.csproj` file, add `<PackageReference Include="PackageName" />` (without a version attribute).

## 🛠 Common Tasks

### Adding a new Asset Type
1.  Update the `AssetType` enum in `InvestmentTracker.Domain`.
2.  If necessary, add logic in `FeeCalculator` or other services.
3.  Add a migration if the database schema is affected (usually Enums are stored as int or string, so verify conversion).

### Adding a new Feature
1.  Define the Entity or Service Interface in **Domain**.
2.  Implement the Interface in **Infra** (if it involves I/O).
3.  Implement the Logic in **Domain Services** (if it's pure business logic).
4.  Expose/Use the feature in **UI**.
5.  Write tests in **Tests**.

## 🐛 Troubleshooting

-   **EF Core Tools not found**: Ensure you have installed the tools (`dotnet tool install --global dotnet-ef`) and that your PATH is configured.
-   **Database locked**: Ensure no other process (like a DB viewer) has the `investments.db` file open.
