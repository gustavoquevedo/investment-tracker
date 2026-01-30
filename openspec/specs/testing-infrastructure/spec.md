## Requirements

### Requirement: Central Package Management
The solution MUST use Central Package Management (CPM) via `Directory.Packages.props` at the repository root to centralize all NuGet package versions.

#### Scenario: Package version centralization
- **WHEN** any `.csproj` file references a NuGet package
- **THEN** the package reference MUST NOT include a `Version` attribute, and the version MUST be defined in `Directory.Packages.props`

#### Scenario: CPM configuration
- **WHEN** `Directory.Packages.props` is created
- **THEN** it SHALL contain `<ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>` and all `<PackageVersion>` entries for the solution

### Requirement: xUnit v3 Testing Framework
The test project MUST use xUnit v3 as the unit testing framework.

#### Scenario: xUnit v3 package reference
- **WHEN** the test project is configured
- **THEN** it SHALL reference `xunit.v3` and `xunit.v3.runner.visualstudio` packages

#### Scenario: xUnit v3 compatibility
- **WHEN** tests are executed
- **THEN** all existing test patterns (`[Fact]`, `[Theory]`) SHALL work without modification

### Requirement: FluentAssertions Library
The test project MUST use FluentAssertions for all assertion statements.

#### Scenario: FluentAssertions package reference
- **WHEN** the test project is configured
- **THEN** it SHALL reference the `FluentAssertions` package

#### Scenario: Assertion syntax migration
- **WHEN** existing `Assert.*` statements are migrated
- **THEN** they SHALL be replaced with equivalent FluentAssertions syntax (e.g., `Assert.Equal(expected, actual)` becomes `actual.Should().Be(expected)`)

### Requirement: Microsoft Testing Platform Runner
The test project MAY be configured to use Microsoft.Testing.Platform (MTP) for standalone test execution.

#### Scenario: MTP configuration
- **WHEN** MTP is enabled
- **THEN** the test project SHALL include `<TestExecutable>true</TestExecutable>` and `<UseMicrosoftTestingPlatform>true</UseMicrosoftTestingPlatform>` properties

#### Scenario: Test execution compatibility
- **WHEN** tests are run via `dotnet test`
- **THEN** the standard test execution command SHALL continue to work
