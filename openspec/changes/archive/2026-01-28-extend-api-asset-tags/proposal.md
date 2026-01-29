## Why

To improve asset organization and discoverability, users need to see tags associated with assets and filter assets by those tags. This enhancement enables more flexible portfolio management and reporting.

## What Changes

- **Update AssetResponse**: Include the list of tags associated with an asset in the API response.
- **Filter by Tag**: Add functionality to retrieve all assets associated with a specific tag (e.g., via a query parameter or dedicated endpoint).

## Capabilities

### New Capabilities
<!-- Capabilities being introduced. Replace <name> with kebab-case identifier (e.g., user-auth, data-export, api-rate-limiting). Each creates specs/<name>/spec.md -->
<!-- No new capabilities, just extending existing ones. -->

### Modified Capabilities
<!-- Existing capabilities whose REQUIREMENTS are changing (not just implementation).
     Only list here if spec-level behavior changes. Each needs a delta spec file.
     Use existing spec names from openspec/specs/. Leave empty if no requirement changes. -->
- `api-layer`: Add tag information to asset responses and enable tag-based filtering.

## Impact

- **API Consumers**: Will receive additional data in asset responses and can now filter by tag.
- **Backend**: Update `AssetResponse` DTO and `GetAssets` query logic.
- **Database**: Ensure efficient querying of assets by tag (check indices).
