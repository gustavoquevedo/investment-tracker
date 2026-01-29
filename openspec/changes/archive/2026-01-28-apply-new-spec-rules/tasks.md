## 1. Analysis and Setup

- [x] 1.1 Run build and existing tests to ensure clean baseline.
- [x] 1.2 Scan codebase for non-English comments, file names, and documentation.
- [x] 1.3 Identify areas missing tests or violating TDD principles (e.g., untestable logic).

## 2. Domain Layer Compliance

- [x] 2.1 Refactor Domain entities and enums to ensure strict English naming and comments.
- [x] 2.2 Fill test gaps in Domain layer (ensure 100% coverage for business logic).
- [x] 2.3 Verify type safety and nullability settings in Domain project.

## 3. Infrastructure Layer Compliance

- [x] 3.1 Refactor Infrastructure repositories and contexts for English compliance.
- [x] 3.2 Ensure integration tests cover all repositories and EF Core configurations.
- [x] 3.3 Verify compliance with backend standards (migrations, connection handling).

## 4. UI Layer Compliance

- [x] 4.1 Refactor UI components and pages for English compliance.
- [x] 4.2 Ensure UI logic (ViewModels/Code-behind) is unit tested.
- [x] 4.3 Verify compliance with frontend standards (structure, naming).

## 5. Final Verification

- [x] 5.1 Run full test suite to ensure no regressions.
- [x] 5.2 Perform a final manual review of random files against `base-standards.mdc`.
