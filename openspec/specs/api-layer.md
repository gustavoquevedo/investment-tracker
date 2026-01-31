# Capability: api-layer

## Purpose
This spec defines the core capabilities of the API layer, updated to reflect the migration from Controllers to Vertical Slice Architecture.

## Requirements

### Requirement: Manage Assets
The system SHALL provide REST endpoints to create, read, update, and delete assets, including tag support.

> **Implementation Note**: Endpoints are now implemented as individual slices in `Features/Assets/` rather than a single `AssetsController`.

#### Scenario: List all assets
- **WHEN** a GET request is made to `/assets`
- **THEN** the system returns a list of all assets with their details (including associated tags)
- **AND** the endpoint is implemented in `Features/Assets/GetAssets.cs`

#### Scenario: Filter assets by tag
- **WHEN** a GET request is made to `/assets` with a `tag` query parameter (e.g., `?tag=HighRisk`)
- **THEN** the system returns only assets associated with the specified tag

#### Scenario: Create an asset
- **WHEN** a POST request is made to `/assets` with valid asset data
- **THEN** the system creates the asset and returns the created resource with a 201 status
- **AND** the endpoint is implemented in `Features/Assets/CreateAsset.cs`

#### Scenario: Get single asset
- **WHEN** a GET request is made to `/assets/{id}`
- **THEN** the system returns the asset details (including associated tags) if found, or 404 if not found

#### Scenario: Update asset
- **WHEN** a PUT request is made to `/assets/{id}` with valid data
- **THEN** the system updates the asset and returns the updated resource

#### Scenario: Delete asset
- **WHEN** a DELETE request is made to `/assets/{id}`
- **THEN** the system removes the asset and returns 204 No Content

### Requirement: Manage Asset Snapshots
The system SHALL allow viewing and adding snapshots for a specific asset.

> **Implementation Note**: Snapshot endpoints are in `Features/Assets/GetSnapshots.cs` and `Features/Assets/AddSnapshot.cs`.

#### Scenario: List snapshots
- **WHEN** a GET request is made to `/assets/{id}/snapshots`
- **THEN** the system returns a history of value snapshots for that asset

#### Scenario: Add snapshot
- **WHEN** a POST request is made to `/assets/{id}/snapshots` with a date and value
- **THEN** the system records the snapshot and returns 201 Created

### Requirement: Manage Asset Contributions
The system SHALL allow viewing and adding contributions for a specific asset.

> **Implementation Note**: Contribution endpoints are in `Features/Assets/GetContributions.cs` and `Features/Assets/AddContribution.cs`.

#### Scenario: List contributions
- **WHEN** a GET request is made to `/assets/{id}/contributions`
- **THEN** the system returns a list of contributions for that asset

#### Scenario: Add contribution
- **WHEN** a POST request is made to `/assets/{id}/contributions` with amount and date
- **THEN** the system records the contribution and returns 201 Created

### Requirement: Manage Tags
The system SHALL provide endpoints to manage asset tags.

> **Implementation Note**: Tag endpoints are in `Features/Tags/`.

#### Scenario: List tags
- **WHEN** a GET request is made to `/tags`
- **THEN** the system returns all available tags

#### Scenario: Create tag
- **WHEN** a POST request is made to `/tags`
- **THEN** the system creates the tag and returns 201 Created

#### Scenario: Delete tag
- **WHEN** a DELETE request is made to `/tags/{id}`
- **THEN** the system deletes the tag and returns 204 No Content
