# Features Folder (Vertical Slice Architecture)

This folder contains the business capabilities of the Investment Tracker API, organized by **Vertical Slices**.

## Pattern
Each **subdirectory** represents a single API operation (e.g., `GetAssets/`, `CreateAsset/`).
Each slice subdirectory contains:
- **`<Action>Endpoint.cs`**: Defines the route, method, metadata, and handler logic.
- **`<Action>Dtos.cs`**: Request and Response DTOs specific to the endpoint.

## Folder Structure
```text
Features/
├── Assets/
│   ├── GetAssets/
│   │   ├── GetAssetsEndpoint.cs
│   │   └── GetAssetsDtos.cs
│   ├── CreateAsset/
│   │   ├── CreateAssetEndpoint.cs
│   │   └── CreateAssetDtos.cs
│   ├── UpdateAsset/ ...
│   ├── DeleteAsset/ ...
│   ├── GetAssetById/ ...
│   ├── ManageSnapshots/
│   │   ├── GetSnapshotsEndpoint.cs
│   │   ├── AddSnapshotEndpoint.cs
│   │   └── SnapshotsDtos.cs
│   └── ManageContributions/ ...
├── Portfolio/
│   ├── GetSummary/
│   │   ├── GetSummaryEndpoint.cs
│   │   └── SummaryDtos.cs
│   ├── GetHistory/ ...
│   ├── GetAllocation/ ...
│   └── GetReturns/ ...
├── Tags/
│   ├── GetTags/ ...
│   ├── CreateTag/ ...
│   └── DeleteTag/ ...
└── Common/
    └── TagDto.cs (shared DTO)
```

## Principles
1. **Feature First**: Code is grouped by business concern, not technical layer.
2. **One Subdirectory per Operation**: Each endpoint lives in isolation.
3. **Separate Endpoint and DTOs Files**: Clear separation for navigation.
4. **Direct Data Access**: Handlers inject `InvestmentContext` directly.
5. **Shared Types**: Truly common types (like `TagDto`) are in `Common/`.

## Registration
Each feature group has an `<Group>Endpoints.cs` file (e.g., `AssetsEndpoints.cs`) with an extension method to register all slices in `Program.cs`.
