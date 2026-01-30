---
description: .NET 10 SDK features, Central Package Management, solution format, and Entity Framework Core patterns.
globs: ["**/*.csproj", "**/Directory.Packages.props", "**/Directory.Build.props", "**/global.json", "**/*.slnx"]
alwaysApply: true
---

# .NET 10 Tooling Standards

## Technology Stack

- **Framework**: .NET 10 (LTS)
- **Language**: C# 14
- **ORM**: Entity Framework Core 10
- **Testing**: Microsoft.Testing.Platform (MTP), xUnit v3
- **Orchestration**: .NET Aspire (optional)

## Modern .NET Standards

### Solution Format (`.slnx`)

Use the modern `.slnx` solution format instead of legacy `.sln`:

```bash
# Migrate existing solution
dotnet sln migrate
```

### Central Package Management (CPM)

All NuGet package versions are defined in `Directory.Packages.props` at the solution root:

```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  <ItemGroup>
    <PackageVersion Include="Microsoft.EntityFrameworkCore" Version="10.0.0" />
  </ItemGroup>
</Project>
```

In `.csproj` files, reference packages without version:

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" />
```

### Microsoft.Testing.Platform Configuration

Configure MTP in `global.json`:

```json
{
  "test": {
    "runner": "Microsoft.Testing.Platform"
  }
}
```

## Entity Framework Core Patterns

### DbContext Configuration

- Use `DbContext` for database interactions
- Configure entities using `IEntityTypeConfiguration<T>` in separate files
- Keep the Context class clean, avoid `OnModelCreating` logic

### Migrations

```bash
# Add migration
dotnet ef migrations add <MigrationName> --project src/<Infra> --startup-project src/<UI>

# Update database
dotnet ef database update --project src/<Infra> --startup-project src/<UI>
```

## CLI Commands

Use the modern noun-first command syntax:

| Modern (preferred) | Legacy |
|-------------------|--------|
| `dotnet package add` | `dotnet add package` |
| `dotnet package list` | `dotnet list package` |
| `dotnet reference add` | `dotnet add reference` |

## Container Support

Console apps can create container images natively:

```bash
dotnet publish /t:PublishContainer
```

## Build and Restore

```bash
dotnet restore
dotnet build
dotnet test
```
