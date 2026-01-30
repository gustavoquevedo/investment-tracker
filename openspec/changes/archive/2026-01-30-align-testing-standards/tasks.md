## 1. Central Package Management Setup

- [x] 1.1 Create `Directory.Packages.props` at repository root with `<ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>`
- [x] 1.2 Add all current package versions to `Directory.Packages.props` as `<PackageVersion>` entries
- [x] 1.3 Remove `Version` attributes from `src/InvestmentTracker.Infra/InvestmentTracker.Infra.csproj`
- [x] 1.4 Remove `Version` attributes from `src/InvestmentTracker.Api/InvestmentTracker.Api.csproj`
- [x] 1.5 Remove `Version` attributes from `tests/InvestmentTracker.Tests/InvestmentTracker.Tests.csproj`
- [x] 1.6 Verify solution builds successfully with `dotnet build`

## 2. xUnit v3 Upgrade

- [x] 2.1 Update `Directory.Packages.props` to replace `xunit` 2.4.2 with `xunit.v3` 1.0.0
- [x] 2.2 Update `Directory.Packages.props` to replace `xunit.runner.visualstudio` 2.4.5 with `xunit.v3.runner.visualstudio` 1.0.0
- [x] 2.3 Verify all existing tests still compile with xUnit v3
- [x] 2.4 Run `dotnet test` to confirm all tests pass

## 3. FluentAssertions Integration

- [x] 3.1 Add `FluentAssertions` 7.0.0 to `Directory.Packages.props`
- [x] 3.2 Add `<PackageReference Include="FluentAssertions" />` to test project
- [x] 3.3 Migrate `FeeCalculatorTests.cs` from `Assert.*` to FluentAssertions syntax
- [x] 3.4 Migrate `PortfolioServiceTests.cs` from `Assert.*` to FluentAssertions syntax
- [x] 3.5 Run `dotnet test` to confirm all migrated tests pass

## 4. Microsoft Testing Platform Configuration (Optional)

- [x] 4.1 Add MTP properties to test project: `<TestExecutable>true</TestExecutable>` and `<UseMicrosoftTestingPlatform>true</UseMicrosoftTestingPlatform>`
- [x] 4.2 Verify `dotnet test` still works after MTP configuration
- [x] 4.3 Test standalone executable execution if MTP is enabled

## 5. Verification

- [x] 5.1 Run full test suite with `dotnet test`
- [x] 5.2 Verify no `Version` attributes remain in any `.csproj` file
- [x] 5.3 Verify all assertions use FluentAssertions syntax
