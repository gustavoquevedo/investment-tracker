## ADDED Requirements

### Requirement: Fetch all assets with snapshot history
The system SHALL provide an endpoint that returns all assets together with their full snapshot history in a single response.

#### Scenario: Successful fetch
- **WHEN** a GET request is made to `/portfolio/assets-with-snapshots`
- **THEN** the system SHALL return a 200 response with an array of assets, each including its details (id, name, assetType, ticker, isin, feePercentagePerYear) and a nested array of snapshots (snapshotDate, totalValue) ordered by date descending

#### Scenario: No assets exist
- **WHEN** a GET request is made to `/portfolio/assets-with-snapshots` and no assets exist
- **THEN** the system SHALL return a 200 response with an empty array

#### Scenario: Asset with no snapshots
- **WHEN** an asset has no snapshots recorded
- **THEN** the asset SHALL still appear in the response with an empty snapshots array
