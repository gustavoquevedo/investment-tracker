## Why
The application currently consists of Domain and Infrastructure layers but lacks a way to expose this functionality to external clients like a frontend application. Creating a REST API is the necessary next step to allow user interaction with the system.

## What Changes
- Create a new ASP.NET Core Web API project (`InvestmentTracker.Api`).
- Configure Dependency Injection to wire up `InvestmentContext`, repositories, and domain services.
- Implement REST endpoints for Assets and Tags as defined in the `api-spec.yml`.
- Enable Swagger/OpenAPI documentation for API exploration.

## Capabilities

### New Capabilities
- `api-layer`: Expose backend logic via REST endpoints for Assets and Tags management.

### Modified Capabilities
<!-- No existing requirements are changing, we are adding a new layer on top. -->

## Impact
- **New Project**: `src/InvestmentTracker.Api`
- **Dependencies**: The new API project will depend on `InvestmentTracker.Domain` and `InvestmentTracker.Infra`.
