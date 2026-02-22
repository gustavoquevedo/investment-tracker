## Why

The application currently only has a Dashboard view with portfolio-level summaries. There is no way to explore individual assets, see their historical snapshot values side by side, or visualize how each asset's weight has evolved over time. An "Assets" tab provides the detailed, asset-level view that complements the portfolio-level Dashboard.

## What Changes

- Add client-side routing (React Router) to support multiple tabs/pages — currently the app renders `DashboardPage` directly with no router
- Add a top-level tab navigation component (Dashboard, Assets) for switching between views
- Create an **Assets page** with three sections:
  1. **Snapshot table**: A pivot-style table with assets as rows and snapshot dates as columns, showing `totalValue` at each date. Asset detail columns (name, type, ticker, ISIN, fee) appear on the left as frozen/sticky columns. Dates are sortable newest-first. For UX clarity: use horizontal scrolling for many dates rather than trying to fit everything on screen, and highlight the latest snapshot column.
  2. **Stacked area chart (absolute)**: Shows accumulated asset values over time as stacked areas — each area represents one asset's `totalValue`, stacked to show the total portfolio value.
  3. **100% stacked area chart (percentage)**: Shows each asset's proportion of total portfolio value over time. Areas always fill the full vertical space (0–100%).
- Add a new API endpoint to fetch all assets with their snapshots in a single call, avoiding N+1 requests from the frontend

## Capabilities

### New Capabilities
- `assets-view`: The Assets tab UI — snapshot pivot table, stacked area chart (absolute), and 100% stacked area chart (percentage)
- `app-navigation`: Tab-based navigation system enabling multiple pages (Dashboard, Assets, and future tabs)

### Modified Capabilities
- `api-layer`: New endpoint `GET /api/portfolio/assets-with-snapshots` returning all assets with their snapshot history in a single response

## Impact

- **Frontend**: New dependency on `react-router-dom`. App.tsx restructured to use router and layout with tab navigation. New page, components, API hooks, and types added.
- **Backend**: New API endpoint in the portfolio feature area. No domain model changes — reads existing Asset and Snapshot entities.
- **Existing pages**: Dashboard continues to work as-is, just wrapped in the new navigation layout.
