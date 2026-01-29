## 1. Domain & Infrastructure Updates

- [x] 1.1 Update `AssetResponse` DTO to include `List<TagDto> Tags`.
- [x] 1.2 Create `TagDto` class (Id, Name, Color).
- [x] 1.3 Update `AssetRepository` (or equivalent query handler) to Eager Load `AssetTags.Tag` when fetching assets.
- [x] 1.4 Implement filtering logic in `AssetRepository.GetAllAssetsAsync` (or equivalent) to accept an optional `tag` parameter.

## 2. API Layer Updates

- [x] 2.1 Update `GetAssets` endpoint in `AssetsController` (or equivalent) to accept `[FromQuery] string? tag`.
- [x] 2.2 Pass the tag filter down to the service/repository layer.
- [x] 2.3 Verify `AssetResponse` mapping populates the `Tags` list correctly.

## 3. Testing & Verification

- [x] 3.1 Add unit tests for `AssetRepository` filtering logic (filter by existing tag, non-existing tag, null tag).
- [x] 3.2 Update API integration tests to verify `Tags` are returned in the response. (Skipped: No API integration tests project)
- [x] 3.3 Add integration test for filtering assets by tag. (Skipped: No API integration tests project)
