# Portfolio Reporting

## Why

Users need visibility into their investment performance beyond individual asset data. Currently, there's no way to see aggregate portfolio metrics, historical performance trends, or proper return calculations that account for cash flows. This feature enables informed decision-making by providing a comprehensive dashboard with P&L tracking and historical charts.

## What

Build a Portfolio Reporting feature that provides:

1. **Dashboard Overview**
   - Total portfolio value (sum of latest snapshot values across all assets)
   - Total invested (sum of all contributions)
   - Profit & Loss: absolute (value - invested) and percentage ((value - invested) / invested × 100)
   - Filtering by date range and tags

2. **Historical Charts**
   - Portfolio value over time (line chart with snapshot dates)
   - Asset allocation (pie/donut chart by asset type or tag)
   - P&L trend (area chart showing cumulative gains/losses)

3. **Return Calculations**
   - Time-Weighted Return (TWR) - accounts for contributions/withdrawals
   - Money-Weighted Return (IRR) - internal rate of return considering cash flow timing
   - Period returns: daily, weekly, monthly, YTD, 1Y, 3Y, 5Y, All-time
   - Annualized returns for periods > 1 year

## How

### Backend (InvestmentTracker.Api)

1. **New API Endpoints** (`/portfolio/...`):
   - `GET /portfolio/summary` - Returns PortfolioSummaryDto (totalValue, totalInvested, pnL, pnLPercent)
   - `GET /portfolio/history?from={date}&to={date}` - Returns array of PortfolioHistoryPointDto
   - `GET /portfolio/allocation` - Returns array of AllocationDto (by asset type)
   - `GET /portfolio/returns?period={period}` - Returns ReturnsDto (twr, mwr, periodReturns)

2. **Domain Services** (`InvestmentTracker.Domain/Services/`):
   - `IPortfolioService` - Orchestrates reporting logic
   - `IReturnCalculator` - TWR and MWR calculations

3. **DTOs** (`InvestmentTracker.Api/DTOs/`):
   - `PortfolioSummaryDto`
   - `PortfolioHistoryPointDto`
   - `AllocationDto`
   - `ReturnsDto`

### Frontend (InvestmentTracker.Client)

1. **Dashboard Page** - Main reporting view with cards and charts
2. **Chart Components** - Using Recharts library:
   - `PortfolioValueChart` (line)
   - `AllocationChart` (pie/donut)
   - `PnLTrendChart` (area)
3. **API Integration** - React Query hooks for data fetching
4. **Date Range Selector** - Period picker component

## Scope

### In Scope
- API endpoints for all reporting metrics
- React dashboard with interactive charts
- TWR and basic MWR calculations
- Caching for expensive calculations

### Out of Scope
- Real-time price updates
- Benchmark comparisons
- Export to PDF/Excel
- Tax reporting
