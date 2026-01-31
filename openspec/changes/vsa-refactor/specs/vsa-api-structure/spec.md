# Capability: vsa-api-structure

## Purpose
Define the Vertical Slice Architecture (VSA) organization for the API layer, ensuring code is organized by business feature rather than technical layer.

## Requirements

### Requirement: Feature-First Folder Organization
The API project SHALL organize code by business capability in a `Features/` directory, with each endpoint as a self-contained slice.

#### Scenario: Feature folder exists for each capability
- **WHEN** a new API endpoint is added
- **THEN** it is placed in `Features/<Capability>/<ActionName>.cs`
- **AND** the file contains the endpoint definition, request/response DTOs, and handler logic

#### Scenario: No shared Controllers folder
- **WHEN** examining the API project structure
- **THEN** there is no `Controllers/` folder at the project root

#### Scenario: No shared DTOs folder
- **WHEN** examining the API project structure
- **THEN** there is no `DTOs/` folder at the project root (except `Common/` for truly shared types)

### Requirement: Self-Contained Endpoint Slices
Each endpoint slice SHALL contain its own request/response types co-located with the handler.

#### Scenario: DTOs are nested in endpoint class
- **WHEN** viewing an endpoint file (e.g., `GetSummary.cs`)
- **THEN** request and response types are defined as nested records within the endpoint class

#### Scenario: Duplicate DTOs allowed between features
- **WHEN** two features need similar data shapes
- **THEN** each feature defines its own DTO rather than sharing (per "duplication over wrong abstraction" principle)

### Requirement: Minimal API Registration Pattern
Endpoints SHALL use Minimal API with static `Map` methods for registration.

#### Scenario: Endpoint provides Map method
- **WHEN** viewing an endpoint file
- **THEN** it contains a `public static void Map(IEndpointRouteBuilder app)` method

#### Scenario: Feature group provides aggregate registration
- **WHEN** viewing a feature folder (e.g., `Features/Portfolio/`)
- **THEN** it contains a `*Endpoints.cs` file (e.g., `PortfolioEndpoints.cs`) that calls all endpoint `Map` methods

#### Scenario: Program.cs uses extension methods
- **WHEN** viewing `Program.cs`
- **THEN** endpoint registration uses `app.MapAssetEndpoints()`, `app.MapPortfolioEndpoints()`, etc.

### Requirement: Direct DbContext Access
Endpoint handlers SHALL inject `InvestmentContext` directly rather than using repository abstractions for simple CRUD.

#### Scenario: Handler uses DbContext
- **WHEN** an endpoint needs to query or persist data
- **THEN** it receives `InvestmentContext` as a parameter via DI

#### Scenario: No repository interfaces for CRUD
- **WHEN** examining the Domain project interfaces
- **THEN** repository interfaces (`IAssetRepository`, etc.) are removed or only kept for complex queries
