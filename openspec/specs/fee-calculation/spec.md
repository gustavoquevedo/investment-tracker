# Fee Calculation

## Purpose
TBD

## Requirements

### Requirement: Calculate fee for period
The system SHALL calculate the management fee for a given principal amount over a specific period using simple interest approximation: `Principal * (AnnualRate / 365) * Days`.

#### Scenario: Standard calculation
- **WHEN** calculating fee for 1000 principal, 10% annual rate (0.1), and 365 days
- **THEN** the result should be 100

#### Scenario: Partial year
- **WHEN** calculating fee for 1000 principal, 10% annual rate, and 182.5 days
- **THEN** the result should be 50

#### Scenario: Zero days
- **WHEN** duration is 0 days
- **THEN** fee should be 0

#### Scenario: Invalid dates
- **WHEN** start date is after end date
- **THEN** throw ArgumentException
