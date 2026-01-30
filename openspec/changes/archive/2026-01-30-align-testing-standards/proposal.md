## Why

The codebase specifies modern .NET 10 testing standards in `backend-standards.mdc` including xUnit v3, FluentAssertions, Microsoft.Testing.Platform (MTP), and Central Package Management (CPM). However, the current implementation uses outdated xUnit v2.4.2 with inline package versions, creating inconsistency between specifications and actual code. Aligning the testing infrastructure ensures the codebase follows its own documented standards and benefits from modern testing capabilities.

## What Changes

- **Add Central Package Management (CPM)**: Create `Directory.Packages.props` to centralize all NuGet package versions
- **Upgrade to xUnit v3**: Replace xUnit v2.4.2 with xUnit v3 for modern testing features
- **Add FluentAssertions**: Introduce FluentAssertions library and migrate existing assertions
- **Configure MTP Runner**: Enable Microsoft.Testing.Platform for standalone test execution
- **Update all .csproj files**: Remove inline version attributes, reference centralized versions

## Capabilities

### New Capabilities

- `testing-infrastructure`: Defines the testing framework requirements, package versions, and test execution configuration for the project

### Modified Capabilities

- `codebase-compliance`: Adds testing infrastructure alignment to compliance requirements

## Impact

- **Package Management**: All `.csproj` files will be modified to remove `Version` attributes
- **Test Project**: `tests/InvestmentTracker.Tests/InvestmentTracker.Tests.csproj` will reference new packages
- **Test Files**: All test files will be updated from `Assert.*` to FluentAssertions syntax
- **Build Configuration**: New `Directory.Packages.props` and potentially `Directory.Build.props` at solution root
- **CI/CD**: Test execution commands may change with MTP (standalone executable vs `dotnet test`)
