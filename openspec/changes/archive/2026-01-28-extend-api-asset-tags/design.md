## Context

Currently, the `GET /assets` endpoint returns asset details but does not include the tags associated with each asset. There is no way to filter assets by tag via the API. The domain model already supports tags via `AssetTag` and `Tag` entities.

## Goals / Non-Goals

**Goals:**
- Expose asset tags in the `AssetResponse` DTO.
- Enable filtering of assets by a specific tag in the `GET /assets` endpoint.

**Non-Goals:**
- Creating new tag management endpoints (create/update/delete tags) - this is out of scope for this change.
- Complex filtering (e.g., assets with Tag A AND Tag B) - starting with single tag filtering.

## Decisions

### API Design: Query Parameter for Filtering
**Decision:** Use `GET /assets?tag={tagName}` (or `tagId`) for filtering.
**Rationale:** Keeps the API surface clean. Allows extending to multiple tags later.
**Alternative:** Create `GET /tags/{tagId}/assets`. Rejected as it splits asset retrieval logic.

### DTO Design: Include Tags in AssetResponse
**Decision:** Add `List<TagDto> Tags` to `AssetResponse`.
**Rationale:** Clients need to display tags alongside assets.
**Details:** `TagDto` will contain `Id`, `Name`, and `Color` (if available).

### Data Access
**Decision:** Use Eager Loading in EF Core.
**Rationale:** `Include(x => x.AssetTags).ThenInclude(x => x.Tag)` ensures we fetch tags efficiently with the asset.
**Risk:** Potential N+1 if not careful, but `Include` handles it.

## Risks / Trade-offs

- **Performance**: Fetching tags for a large list of assets might increase query time slightly.
  - **Mitigation**: Ensure proper indexing on `AssetTag` table. The dataset is likely small enough that eager loading is fine.
