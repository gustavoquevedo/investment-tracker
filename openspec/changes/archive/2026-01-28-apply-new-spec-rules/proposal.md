## Why

The codebase needs to be updated to reflect the latest development standards and rules defined in `ai-specs/specs/base-standards.mdc`. This ensures consistency, maintainability, and adherence to the project's quality guidelines following recent updates to the specifications.

## What Changes

- Review and refactor code to align with "Core Principles" (Small tasks, TDD, type safety, clear naming, incremental changes).
- Ensure strict adherence to the English-only requirement for all technical artifacts.
- Verify and enforce compliance with backend and frontend specific standards referenced in `base-standards.mdc`.
- Update project configuration or tooling if necessary to support the new standards.

## Capabilities

### New Capabilities
- `codebase-compliance`: Enforce adherence to defined development standards (TDD, naming, etc.) across the codebase.

### Modified Capabilities
<!-- Existing capabilities whose REQUIREMENTS are changing (not just implementation).
     Only list here if spec-level behavior changes. Each needs a delta spec file.
     Use existing spec names from openspec/specs/. Leave empty if no requirement changes. -->

## Impact

- **Codebase-wide**: Potential refactoring across Domain, Infrastructure, and UI layers.
- **Testing**: Tests may need updates to match TDD and type safety standards.
- **Documentation**: Comments and documentation strings may need updates to ensure English-only compliance.
