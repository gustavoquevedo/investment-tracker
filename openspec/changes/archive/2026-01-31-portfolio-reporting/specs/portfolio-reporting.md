# Portfolio Reporting Specifications

## Purpose

Provide users with a comprehensive view of their investment portfolio performance, including aggregate metrics, historical trends, and return calculations.

## Requirements

### Requirement: Get Portfolio Summary

The system SHALL provide a summary of the entire portfolio, aggregating totals from all assets.

#### Scenario: Calculate totals
- **GIVEN** assets exist with snapshots and contributions
- **WHEN** requesting portfolio summary via `GET /portfolio/summary`
- **THEN** `totalInvested` is sum of all contributions across all assets
- **THEN** `totalValue` is sum of the latest snapshot value for each asset
- **THEN** `pnL` is `totalValue` minus `totalInvested`
- **THEN** `pnLPercent` is `(pnL / totalInvested) * 100`
- **THEN** `assetCount` is the count of assets
- **THEN** `lastUpdated` is the most recent snapshot date

#### Scenario: Empty portfolio
- **GIVEN** no assets exist
- **WHEN** requesting portfolio summary
- **THEN** all numeric values are `0`
- **THEN** `lastUpdated` is `null`

---

### Requirement: Get Portfolio History

The system SHALL provide historical portfolio values for charting.

#### Scenario: Get history within date range
- **GIVEN** assets have snapshots on various dates
- **WHEN** requesting history via `GET /portfolio/history?from={date}&to={date}`
- **THEN** return array of points with `date`, `value`, and `invested`
- **THEN** each point aggregates all assets' values on that date
- **THEN** points are ordered by date ascending

#### Scenario: Get all history
- **GIVEN** no date range specified
- **WHEN** requesting history via `GET /portfolio/history`
- **THEN** return last 5 years of data by default

---

### Requirement: Get Portfolio Allocation

The system SHALL provide asset allocation breakdown by type.

#### Scenario: Calculate allocation percentages
- **GIVEN** portfolio has assets of different types
- **WHEN** requesting allocation via `GET /portfolio/allocation`
- **THEN** return array grouped by `assetType`
- **THEN** each entry includes `type`, `value`, and `percentage`
- **THEN** percentages sum to 100

---

### Requirement: Calculate Returns

The system SHALL calculate Time-Weighted Return (TWR) and Money-Weighted Return (MWR).

#### Scenario: Calculate TWR
- **GIVEN** portfolio has snapshots and contributions
- **WHEN** requesting returns via `GET /portfolio/returns`
- **THEN** calculate TWR using sub-period returns between contributions
- **THEN** TWR eliminates effect of cash flow timing

#### Scenario: Calculate period returns
- **WHEN** requesting returns
- **THEN** include returns for: YTD, 1Y, 3Y, 5Y, All-Time
- **THEN** annualize returns for periods > 1 year

---

### Requirement: React Dashboard

The frontend SHALL display an interactive reporting dashboard.

#### Components:
- **PortfolioSummary**: Cards showing totalValue, pnL, pnLPercent
- **PortfolioValueChart**: Line chart of value over time
- **AllocationChart**: Pie/donut chart of asset allocation
- **PeriodSelector**: Dropdown for time period selection

#### Behaviors:
- Dashboard loads summary and charts on mount
- Charts update when period selection changes
- Empty states shown when no data available
