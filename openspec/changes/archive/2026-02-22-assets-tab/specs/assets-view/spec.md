## ADDED Requirements

### Requirement: Assets snapshot table
The system SHALL display a pivot-style table on the Assets page showing all assets with their snapshot values across dates.

#### Scenario: Table renders with asset details and snapshot columns
- **WHEN** the user navigates to the Assets tab
- **THEN** the table displays rows for each asset with sticky left columns (name, type, ticker, ISIN, fee) and one column per unique snapshot date showing the `totalValue`

#### Scenario: Dates ordered newest-first
- **WHEN** the snapshot table renders
- **THEN** date columns SHALL be ordered from newest (leftmost) to oldest (rightmost)

#### Scenario: Latest snapshot column highlighted
- **WHEN** the table renders
- **THEN** the most recent snapshot date column SHALL be visually highlighted to distinguish it from older columns

#### Scenario: Horizontal scroll for many dates
- **WHEN** the number of snapshot date columns exceeds the viewport width
- **THEN** the table SHALL support horizontal scrolling while keeping the asset detail columns (name, type, ticker, ISIN, fee) frozen/sticky on the left

#### Scenario: Empty cell for missing snapshot
- **WHEN** an asset has no snapshot recorded for a given date
- **THEN** the cell SHALL display a dash ("—") or be left empty

### Requirement: Stacked area chart showing absolute asset values
The system SHALL display a stacked area chart showing each asset's `totalValue` over time, stacked to visualize total portfolio value.

#### Scenario: Chart renders with all assets stacked
- **WHEN** the Assets page loads with snapshot data
- **THEN** a stacked area chart SHALL render with one area per asset, each representing its `totalValue` at each snapshot date, stacked so the top line equals the total portfolio value

#### Scenario: Asset identification via legend and tooltip
- **WHEN** the user views the stacked area chart
- **THEN** a legend SHALL identify each asset by name, and hovering over the chart SHALL show a tooltip with the date, each asset's value, and the total

### Requirement: 100% stacked area chart showing asset allocation percentages
The system SHALL display a 100% stacked area chart showing each asset's percentage of total portfolio value over time.

#### Scenario: Chart fills full vertical area (0–100%)
- **WHEN** the Assets page loads with snapshot data
- **THEN** a 100% stacked area chart SHALL render where the areas always span from 0% to 100%, each area representing an asset's proportion of total value at that date

#### Scenario: Tooltip shows percentage and value
- **WHEN** the user hovers over the percentage chart
- **THEN** the tooltip SHALL display the date, each asset's percentage, and its absolute value
