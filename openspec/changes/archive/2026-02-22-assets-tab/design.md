## Context

The application is a single-page React 19 app with no routing. `App.tsx` renders `DashboardPage` directly. The backend uses Vertical Slice Architecture (VSA) with Minimal API endpoints grouped by feature. Portfolio queries currently fetch assets and snapshots separately. The frontend uses Recharts for charts and React Query for data fetching, with plain CSS (no UI library).

## Goals / Non-Goals

**Goals:**
- Add client-side routing with React Router to support multiple pages
- Create a reusable tab navigation layout that accommodates future tabs
- Build the Assets page with snapshot table and two stacked area charts
- Add a single backend endpoint that returns assets with snapshots efficiently

**Non-Goals:**
- Editing assets, snapshots, or contributions from the Assets page (read-only view)
- Filtering or searching within the snapshot table (future enhancement)
- Virtual scrolling for the table (only needed if hundreds of columns — unlikely for monthly snapshots)
- Dark mode or theme support

## Decisions

### 1. React Router for navigation
**Choice**: `react-router-dom` v7 with `BrowserRouter`
**Why**: Standard React routing solution. The frontend standards specify React Router. Simple to set up with two routes (`/` for Dashboard, `/assets` for Assets).
**Alternatives considered**: Hash router (unnecessary — Vite proxy handles SPA fallback), no router with conditional rendering (doesn't support direct URL navigation).

### 2. Layout component with tab navigation
**Choice**: Create a `MainLayout` component in `layouts/` that wraps all pages with a top navigation bar.
**Why**: Follows the `layouts/` convention from frontend standards. Keeps navigation concerns separate from page content. Easy to add more tabs later.
**Structure**: `<MainLayout>` renders a `<nav>` with `<NavLink>` items + an `<Outlet>` for page content.

### 3. Snapshot table as a plain HTML table with CSS sticky columns
**Choice**: Native `<table>` with `position: sticky` on the left detail columns and `overflow-x: auto` on the container.
**Why**: No need for a table library — the data structure is straightforward (assets × dates). CSS sticky is well-supported. Keeps dependencies minimal.
**Alternatives considered**: AG Grid / TanStack Table (overkill for a read-only pivot table with no sorting/filtering/pagination requirements).

### 4. Recharts for both stacked area charts
**Choice**: Reuse Recharts (already a dependency) with `<AreaChart>` and `stackId` prop.
**Why**: Already used for Dashboard charts. Supports stacked areas natively. For the 100% chart, precompute percentages in the data transformation layer.
**Data transformation**: Transform the API response into chart-ready format in a custom hook — one data point per date with a property per asset.

### 5. New backend endpoint as a VSA slice
**Choice**: `GET /portfolio/assets-with-snapshots` under `Features/Portfolio/GetAssetsWithSnapshots/`.
**Why**: Fits the existing VSA pattern. Keeps it in the Portfolio group since it's a portfolio-level view. Uses EF Core `.Include(a => a.Snapshots)` for eager loading instead of N+1 queries.
**Response shape**:
```json
[
  {
    "id": 1,
    "name": "S&P 500 ETF",
    "assetType": "ETF",
    "ticker": "SPY",
    "isin": "...",
    "feePercentagePerYear": 0.0009,
    "snapshots": [
      { "snapshotDate": "2025-12-01", "totalValue": 15000.00 },
      { "snapshotDate": "2025-11-01", "totalValue": 14500.00 }
    ]
  }
]
```

### 6. Data transformation in a custom hook
**Choice**: `useAssetsWithSnapshots()` hook in `api/hooks.ts` fetches data via React Query. A separate `useAssetsChartData()` hook transforms the response into chart-ready formats (absolute stacked and percentage stacked).
**Why**: Separates API concerns from presentation concerns. The transformation (pivoting dates, computing percentages) is pure logic that can be tested independently.

## Risks / Trade-offs

- **[Large number of snapshot dates]** → Horizontal scrolling handles this for the table. Charts may become noisy with many data points, but Recharts handles this gracefully with responsive containers. If performance becomes an issue, date range filtering can be added later.
- **[React Router adds a dependency]** → Required by the frontend standards and necessary for proper URL-based navigation. Minimal overhead.
- **[CSS sticky columns browser support]** → Well-supported in all modern browsers. No IE11 concern for a personal finance app.
