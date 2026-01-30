## Requirements

### Requirement: Base Standards Compliance
The codebase MUST adhere to the development standards defined in `ai-specs/specs/base-standards.mdc`, including TDD, type safety, English-only artifacts, and testing infrastructure alignment.

#### Scenario: Code verification
- **WHEN** the codebase is analyzed for compliance with `base-standards.mdc`
- **THEN** no violations (such as non-English comments, missing tests for new code, or loose typing) are found

#### Scenario: Testing infrastructure verification
- **WHEN** the testing setup is analyzed for compliance with `backend-standards.mdc`
- **THEN** the test project SHALL use xUnit v3, FluentAssertions, and Central Package Management as mandated
