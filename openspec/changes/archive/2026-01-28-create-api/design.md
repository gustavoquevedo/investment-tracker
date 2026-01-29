## Context
The application is currently a Domain and Infrastructure library with a CLI/Console entry point. To support a future frontend or other consumers, we need a standard REST API. The `api-spec.yml` file already exists and defines the contract we must fulfill.

## Goals / Non-Goals
**Goals:**
- Create a new `InvestmentTracker.Api` project.
- Implement all endpoints defined in `api-spec.yml`.
- Connect the API to the existing SQLite database via `InvestmentTracker.Infra`.
- Expose Swagger UI for easy testing and verification.

**Non-Goals:**
- Authentication/Authorization (out of scope for now, as per current spec).
- Changing domain logic (we are just exposing it).

## Decisions
- **Framework**: ASP.NET Core Web API 8.0/10.0 (match existing projects).
- **Dependency Injection**: Use built-in .NET DI container to register Repositories and Services.
- **Database Access**: Reuse `InvestmentContext` from `InvestmentTracker.Infra`. Ensure the connection string points to the correct `investments.db` location (likely strictly relative or configured via `appsettings.json`).
- **DTO Mapping**: We will manually map Domain entities to API DTOs (or use a lightweight mapper if complex) to keep it simple and explicit. The `api-spec.yml` defines the DTO structure.

## Risks / Trade-offs
- **Risk**: Database Locking. SQLite can lock if multiple processes (Console App + API) try to write.
  - **Mitigation**: For dev, ensure only one is running or accept the risk. This is a single-user app for now.
- **Risk**: Connection String Path.
  - **Mitigation**: Use `appsettings.json` and ensure the path is correct relative to the execution context.
