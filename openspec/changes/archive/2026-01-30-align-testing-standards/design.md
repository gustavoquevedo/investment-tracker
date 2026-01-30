## Context

The Investment Tracker project specifies modern .NET 10 testing standards in `backend-standards.mdc`:
- **Testing Framework**: Microsoft.Testing.Platform (MTP), xUnit v3
- **Assertion Library**: FluentAssertions
- **Package Management**: Central Package Management (CPM)

Currently, the codebase uses:
- xUnit v2.4.2 with inline package versions
- Standard xUnit `Assert.*` methods
- No CPM (versions scattered across `.csproj` files)

This creates drift between documented standards and actual implementation.

## Goals / Non-Goals

**Goals:**
- Implement Central Package Management (CPM) via `Directory.Packages.props`
- Upgrade from xUnit v2.4.2 to xUnit v3
- Add FluentAssertions and migrate existing assertions
- Prepare for MTP runner (optional configuration)
- Maintain backward compatibility with `dotnet test`

**Non-Goals:**
- Changing test logic or coverage
- Adding new tests (infrastructure change only)
- Modifying CI/CD pipelines (may require separate change)
- Upgrading other application dependencies (focus on test infrastructure)

## Decisions

### Decision 1: CPM via Directory.Packages.props

**Choice**: Create `Directory.Packages.props` at repository root with all package versions centralized.

**Rationale**: This is the .NET 10 recommended approach. It simplifies version management, ensures consistency across projects, and makes upgrades easier.

**Alternatives considered**:
- Keep inline versions → Rejected: Violates documented standards, harder to maintain
- Use `Directory.Build.props` only → Rejected: CPM provides dedicated, cleaner solution

### Decision 2: xUnit v3 Migration

**Choice**: Replace `xunit` 2.4.2 with `xunit.v3` 1.0.x packages.

**Rationale**: xUnit v3 is specified in `backend-standards.mdc`. It offers improved performance, better async support, and aligns with MTP.

**Alternatives considered**:
- Stay on xUnit v2 → Rejected: Violates documented standards
- Switch to NUnit/MSTest → Rejected: Would require rewriting all tests, not specified in standards

### Decision 3: FluentAssertions Syntax

**Choice**: Add FluentAssertions package and migrate existing assertions.

**Rationale**: FluentAssertions provides more readable assertions and better error messages. Specified in `backend-standards.mdc`.

**Migration pattern**:
| xUnit | FluentAssertions |
|-------|------------------|
| `Assert.Equal(expected, actual)` | `actual.Should().Be(expected)` |
| `Assert.True(condition)` | `condition.Should().BeTrue()` |
| `Assert.Throws<T>(action)` | `action.Should().Throw<T>()` |

### Decision 4: MTP Configuration (Optional)

**Choice**: Add MTP properties to test project but keep standard `dotnet test` working.

**Rationale**: MTP enables standalone test execution but transition should be gradual. Adding properties now prepares for future adoption.

## Risks / Trade-offs

| Risk | Mitigation |
|------|------------|
| xUnit v3 breaking changes | Review migration guide; test all existing tests pass after upgrade |
| FluentAssertions learning curve | Syntax is intuitive; provide migration examples in tasks |
| CPM version conflicts | Audit all `.csproj` files for version mismatches before migration |
| Build failures during migration | Migrate in atomic commits; verify build after each step |
