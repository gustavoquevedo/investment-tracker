## 1. Backend — Assets with Snapshots Endpoint

- [x] 1.1 Create `Features/Portfolio/GetAssetsWithSnapshots/` folder with `GetAssetsWithSnapshotsEndpoint.cs` and `GetAssetsWithSnapshotsDtos.cs`
- [x] 1.2 Implement the endpoint: query all assets with `.Include(a => a.Snapshots)`, map to DTOs, return snapshots ordered by date descending
- [x] 1.3 Register the endpoint in `PortfolioEndpoints.cs` as `GET /portfolio/assets-with-snapshots`
- [x] 1.4 Verify the endpoint works via manual test or integration test

## 2. Frontend — Routing and Navigation

- [x] 2.1 Install `react-router-dom` dependency
- [x] 2.2 Create `layouts/MainLayout.tsx` with a top navigation bar (`NavLink` for Dashboard and Assets) and `<Outlet>` for page content
- [x] 2.3 Add CSS styles for the navigation bar and active tab indicator in `index.css`
- [x] 2.4 Refactor `App.tsx` to use `BrowserRouter` with routes: `/` → `DashboardPage`, `/assets` → `AssetsPage`, wrapped in `MainLayout`
- [x] 2.5 Configure Vite to handle SPA fallback for client-side routing (if not already configured)

## 3. Frontend — API Layer for Assets

- [x] 3.1 Add TypeScript types for the assets-with-snapshots response in `api/types.ts` (`AssetWithSnapshots`, `SnapshotEntry`)
- [x] 3.2 Add `useAssetsWithSnapshots()` React Query hook in `api/hooks.ts`

## 4. Frontend — Snapshot Table Component

- [x] 4.1 Create `pages/AssetsPage.tsx` as the container page that composes the table and charts
- [x] 4.2 Create `components/assets/SnapshotTable.tsx` — pivot table with sticky left columns (name, type, ticker, ISIN, fee) and scrollable date columns
- [x] 4.3 Add CSS styles for the snapshot table: sticky columns, horizontal scroll container, latest-date highlight, dash for missing values

## 5. Frontend — Stacked Area Charts

- [x] 5.1 Create data transformation hook `useAssetsChartData()` that converts API response into chart-ready formats (absolute values and percentages per date)
- [x] 5.2 Create `components/charts/AssetsStackedChart.tsx` — stacked area chart (absolute values) with legend and tooltips
- [x] 5.3 Create `components/charts/AssetsPercentageChart.tsx` — 100% stacked area chart with legend and tooltips showing both percentage and absolute value

## 6. Integration and Polish

- [x] 6.1 Wire up `AssetsPage` with the data hook, table, and both charts
- [x] 6.2 Add loading and empty states for the Assets page
- [x] 6.3 Verify end-to-end: navigation works, table renders correctly, charts display data, direct URL `/assets` works
