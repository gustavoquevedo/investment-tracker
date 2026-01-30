## 1. Update DTO

- [x] 1.1 Add `TotalFees` property to `PortfolioSummary.cs`
- [x] 1.2 Add `NetPnL` property to `PortfolioSummary.cs`

## 2. Implement Fee Integration

- [x] 2.1 In `GetPortfolioSummaryAsync()`, calculate fees for each asset
- [x] 2.2 Set `TotalFees` and `NetPnL` in the returned summary

## 3. Update Tests

- [x] 3.1 Update `GetPortfolioSummaryAsync_ShouldCalculateTotalsCorrectly` to mock fee calculation
- [x] 3.2 Add new test verifying `TotalFees` and `NetPnL` are calculated correctly

## 4. Verification

- [x] 4.1 Run `dotnet test` and ensure all tests pass
