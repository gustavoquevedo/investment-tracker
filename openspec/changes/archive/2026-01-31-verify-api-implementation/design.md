## Context
The REST API has been implemented (`InvestmentTracker.Api`) with controllers for Assets and Tags. The `api-spec.yml` defines 13 endpoints but there are no integration tests to verify they work correctly through the HTTP layer. Unit tests exist for domain services but don't cover the API layer.

## Goals / Non-Goals
**Goals:**
- Create API integration tests using `WebApplicationFactory<Program>`
- Test all 13 endpoints for correct HTTP behavior
- Verify CRUD operations work end-to-end with in-memory database
- Ensure correct status codes and response shapes

**Non-Goals:**
- Performance or load testing
- Changing existing API implementation (unless bugs found)
- Authentication testing (not yet implemented)

## Decisions
- **Test Project**: Create `InvestmentTracker.Api.Tests` as separate test project
- **Test Framework**: xUnit + FluentAssertions (matches existing tests)
- **Database**: Use in-memory SQLite for test isolation
- **Pattern**: One test class per controller with methods per endpoint

## Risks / Trade-offs
- **Risk**: In-memory SQLite may behave differently than file-based
  - **Mitigation**: Acceptable for integration testing; file-based tests can be added later if needed
