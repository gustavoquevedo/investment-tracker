## ADDED Requirements

### Requirement: Get Portfolio Summary
The system SHALL provide a summary of the entire portfolio, aggregating totals from all assets.

#### Scenario: Calculate totals
- **WHEN** requesting portfolio summary
- **THEN** TotalInvested is sum of all contributions
- **THEN** TotalValue is sum of latest snapshot value for each asset
- **THEN** TotalPnL is TotalValue minus TotalInvested
