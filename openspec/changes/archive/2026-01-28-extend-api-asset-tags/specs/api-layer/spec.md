## MODIFIED Requirements

### Requirement: Manage Assets
The system SHALL provide REST endpoints to create, read, update, and delete assets, including tag support.

#### Scenario: List all assets
- **WHEN** a GET request is made to `/assets`
- **THEN** the system returns a list of all assets with their details (including associated tags)

#### Scenario: Filter assets by tag
- **WHEN** a GET request is made to `/assets` with a `tag` query parameter (e.g., `?tag=HighRisk`)
- **THEN** the system returns only assets associated with the specified tag

#### Scenario: Create an asset
- **WHEN** a POST request is made to `/assets` with valid asset data
- **THEN** the system creates the asset and returns the created resource with a 201 status

#### Scenario: Get single asset
- **WHEN** a GET request is made to `/assets/{id}`
- **THEN** the system returns the asset details (including associated tags) if found, or 404 if not found

#### Scenario: Update asset
- **WHEN** a PUT request is made to `/assets/{id}` with valid data
- **THEN** the system updates the asset and returns the updated resource

#### Scenario: Delete asset
- **WHEN** a DELETE request is made to `/assets/{id}`
- **THEN** the system removes the asset and returns 204 No Content
