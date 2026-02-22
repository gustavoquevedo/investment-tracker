# Capability: app-navigation

## Purpose
Provide tab-based client-side navigation enabling users to switch between application pages.

## Requirements

### Requirement: Tab-based navigation
The system SHALL provide a tab-based navigation bar enabling the user to switch between application pages.

#### Scenario: Navigation bar displays available tabs
- **WHEN** the application loads
- **THEN** a navigation bar SHALL display tabs for "Dashboard" and "Assets"

#### Scenario: Active tab indication
- **WHEN** the user is on a page
- **THEN** the corresponding tab SHALL be visually highlighted as active

#### Scenario: Navigate between tabs
- **WHEN** the user clicks a tab
- **THEN** the application SHALL navigate to the corresponding page without a full page reload (client-side routing)

#### Scenario: Direct URL navigation
- **WHEN** the user navigates directly to a URL (e.g., `/assets`)
- **THEN** the correct page SHALL render and the corresponding tab SHALL be active

#### Scenario: Default route
- **WHEN** the user navigates to the root URL (`/`)
- **THEN** the application SHALL display the Dashboard page
