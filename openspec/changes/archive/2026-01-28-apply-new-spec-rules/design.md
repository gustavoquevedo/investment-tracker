## Context

The project has adopted strict development standards defined in `ai-specs/specs/base-standards.mdc`. The current codebase may predate some of these rules or have drifted from them. This change aims to align the entire codebase with these authoritative standards to ensure long-term maintainability and consistency.

## Goals / Non-Goals

**Goals:**
- Ensure all code follows the "Core Principles" (TDD, type safety, etc.) from `base-standards.mdc`.
- Enforce English-only language in all technical artifacts.
- Verify compliance with backend and frontend specific standards.
- Maintain functional parity (no regressions).

**Non-Goals:**
- Adding new functional features.
- Major architectural rewrites unless strictly required by the standards.
- Changing code that is already compliant "just because".

## Decisions

### Approach: Layer-by-Layer Compliance
We will apply changes following the dependency graph: Domain -> Infrastructure -> UI.
- **Rationale**: This minimizes ripple effects. Changes in the Domain often force changes in upper layers. Fixing the foundation first is safer.
- **Alternative**: Rule-by-rule (e.g., fix all naming, then all tests). Rejected because it would involve context switching across the entire codebase repeatedly.

### Validation: Test-Driven Refactoring
For any refactoring required to meet standards:
1. Ensure existing tests cover the code.
2. If not, write tests first (TDD).
3. Refactor to meet standards.
4. Verify tests pass.
- **Rationale**: Strictly follows the TDD principle in `base-standards.mdc` and mitigates regression risks.

## Risks / Trade-offs

- **Risk: Regression**: Refactoring working code can introduce bugs.
  - **Mitigation**: Strict adherence to TDD. Run full test suite after each layer is updated.

- **Risk: Interpretation of Standards**: Some standards might be subjective (e.g., "clear naming").
  - **Mitigation**: When in doubt, favor verbosity and explicitness. Consult `base-standards.mdc` examples.
