# Portfolio Management

## Purpose
TBD

## Requirements

### Requirement: Create Asset
The system SHALL allow creating a new asset with a name and type.

#### Scenario: Successful creation
- **WHEN** creating an asset with valid name "Apple Stock"
- **THEN** the asset is persisted and returned with an ID

#### Scenario: Empty name
- **WHEN** creating an asset with empty name
- **THEN** validation fails

### Requirement: Add Contribution
The system SHALL allow adding a contribution to an existing asset.

#### Scenario: Valid contribution
- **WHEN** adding a contribution to existing asset ID 1
- **THEN** the contribution is persisted linked to asset 1

#### Scenario: Missing asset
- **WHEN** adding a contribution to non-existent asset ID 999
- **THEN** throw KeyNotFoundException

### Requirement: Add Snapshot
The system SHALL allow adding a value snapshot to an existing asset.

#### Scenario: Valid snapshot
- **WHEN** adding a snapshot to existing asset ID 1
- **THEN** the snapshot is persisted
