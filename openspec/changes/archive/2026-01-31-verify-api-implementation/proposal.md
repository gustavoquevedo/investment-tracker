## Why
The API layer (`InvestmentTracker.Api`) has been implemented but has not been end-to-end verified against the OpenAPI spec (`ai-specs/project/api-spec.yml`). Before moving to Portfolio Reporting, we need to ensure all CRUD operations work correctly through the HTTP layer.

## What Changes
- Add API integration tests using `WebApplicationFactory` for end-to-end HTTP testing
- Verify all 13 endpoints defined in `api-spec.yml` are correctly implemented
- Test CRUD operations for Assets, Snapshots, Contributions, and Tags
- Ensure proper HTTP status codes (200, 201, 204, 404) and response shapes

## Capabilities

### New Capabilities
- `api-integration-tests`: Comprehensive test coverage for the REST API layer

### Modified Capabilities
<!-- No existing capabilities are being modified -->

## Impact
- **New Project**: `tests/InvestmentTracker.Api.Tests` - Integration test project
- **Dependencies**: Microsoft.AspNetCore.Mvc.Testing for WebApplicationFactory
