# Tasks: Portfolio Reporting

## Backend Tasks

### Task 1: Create DTOs for Portfolio Reporting
**Estimate**: 30 minutes

Create data transfer objects in `src/InvestmentTracker.Api/DTOs/Portfolio/`:

- [x] `PortfolioSummaryDto.cs` - totalValue, totalInvested, pnL, pnLPercent, assetCount, lastUpdated
- [x] `PortfolioHistoryDto.cs` - points array with date, value, invested
- [x] `PortfolioAllocationDto.cs` - byType array with type, value, percentage
- [x] `PortfolioReturnsDto.cs` - twr, mwr, periods object

**Reference**: [design.md](./design.md) → API Endpoints section

---

### Task 2: Create IPortfolioService Interface
**Estimate**: 15 minutes

Create interface in `src/InvestmentTracker.Domain/Services/`:

- [x] `IPortfolioService.cs` with methods:
  - `GetSummaryAsync()`
  - `GetHistoryAsync(DateOnly? from, DateOnly? to)`
  - `GetAllocationAsync()`
  - `GetReturnsAsync(DateOnly? asOf)`

**Reference**: [ai-specs/base/csharp-standards.md](file:///d:/repo/2026/investment-tracker/ai-specs/base/csharp-standards.md)

---

### Task 3: Implement PortfolioService
**Estimate**: 1.5 hours

Implement in `src/InvestmentTracker.Domain/Services/`:

- [x] `PortfolioService.cs` implementing `IPortfolioService`
- [x] Summary: Aggregate latest snapshots and contributions
- [x] History: Query snapshots within date range, group by date
- [x] Allocation: Group assets by type, calculate percentages
- [x] Register in DI container

**Reference**: [domain-model.md](file:///d:/repo/2026/investment-tracker/ai-specs/project/domain-model.md)

---

### Task 4: Implement ReturnCalculator
**Estimate**: 1 hour

Create in `src/InvestmentTracker.Domain/Services/`:

- [x] ~~`IReturnCalculator.cs` interface~~ (integrated into PortfolioService)
- [x] Return calculations in `PortfolioService.GetReturnsAsync()`:
  - TWR calculation using simple percentage
  - Period return calculations (YTD, 1Y, 3Y, 5Y, All-Time)
  - Annualization for multi-year periods

**Reference**: [design.md](./design.md) → Return Calculation Formulas

---

### Task 5: Create PortfolioController
**Estimate**: 45 minutes

Create in `src/InvestmentTracker.Api/Controllers/`:

- [x] `PortfolioController.cs` with endpoints:
  - `GET /portfolio/summary`
  - `GET /portfolio/history`
  - `GET /portfolio/allocation`
  - `GET /portfolio/returns`
- [x] Add proper route attributes and response types
- [x] Handle edge cases (empty portfolio, invalid dates)

**Reference**: [api-spec.yml](file:///d:/repo/2026/investment-tracker/ai-specs/project/api-spec.yml)

---

### Task 6: Add Portfolio API Tests
**Estimate**: 1 hour

Create tests in `tests/InvestmentTracker.Api.Tests/`:

- [x] `PortfolioControllerTests.cs` with integration tests for all endpoints
- [x] Test empty portfolio scenario
- [x] Test with sample data (assets, snapshots, contributions)

**Reference**: [ai-specs/base/testing-standards.md](file:///d:/repo/2026/investment-tracker/ai-specs/base/testing-standards.md)

---

## Frontend Tasks

### Task 7: Install Recharts
**Estimate**: 10 minutes

In `src/InvestmentTracker.Client/`:

- [ ] `npm install recharts`
- [ ] Verify TypeScript types available

---

### Task 8: Create Portfolio API Hooks
**Estimate**: 30 minutes

Create in `src/InvestmentTracker.Client/src/api/`:

- [ ] `usePortfolioSummary.ts` - React Query hook for summary
- [ ] `usePortfolioHistory.ts` - Hook with date range params
- [ ] `usePortfolioAllocation.ts` - Hook for allocation data
- [ ] `usePortfolioReturns.ts` - Hook for returns data

---

### Task 9: Create Chart Components
**Estimate**: 1 hour

Create in `src/InvestmentTracker.Client/src/components/charts/`:

- [ ] `PortfolioValueChart.tsx` - Line chart for value over time
- [ ] `AllocationChart.tsx` - Pie/donut chart for allocation
- [ ] `PnLTrendChart.tsx` - Area chart for P&L trend

---

### Task 10: Create Dashboard Page
**Estimate**: 1 hour

Create in `src/InvestmentTracker.Client/src/pages/`:

- [ ] `DashboardPage.tsx` with:
  - Summary cards (total value, P&L, etc.)
  - Chart grid layout
  - Period selector
  - Loading and empty states
- [ ] Add route to dashboard

---

## Summary

| Area | Tasks | Estimated Time |
|------|-------|----------------|
| Backend | 6 | ~5 hours |
| Frontend | 4 | ~2.5 hours |
| **Total** | **10** | **~7.5 hours** |
