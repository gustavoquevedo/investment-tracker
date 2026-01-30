## ADDED Requirements

### Requirement: Base tier directory structure

The `ai-specs/base/` directory SHALL contain reusable standards organized by concern:

| File | Purpose |
|------|---------|
| `README.md` | Instructions for using the base tier in new projects |
| `csharp-standards.mdc` | C# 14 coding guidelines, naming conventions, async patterns |
| `dotnet-tooling.mdc` | .NET 10 SDK features, CPM, .slnx, EF Core patterns |
| `architecture.mdc` | VSA + Clean Architecture + DDD patterns |
| `testing-standards.mdc` | MTP, xUnit v3, FluentAssertions, Moq usage |
| `frontend-standards.mdc` | React/TypeScript/Vite standards |
| `documentation.mdc` | AI specs, documentation rules, language requirements |

#### Scenario: New project setup

- **WHEN** a developer creates a new .NET 10 project
- **THEN** they can copy the entire `ai-specs/base/` folder to get consistent standards

#### Scenario: Base files have correct globs

- **WHEN** an AI agent processes files in a new project using base standards
- **THEN** the globs in base `.mdc` files SHALL use generic patterns (e.g., `**/*.cs`) not project-specific paths

---

### Requirement: Project tier directory structure

The `ai-specs/project/` directory SHALL contain Investment Tracker-specific documentation:

| File | Purpose |
|------|---------|
| `architecture.mdc` | Project-specific layer mapping and folder structure |
| `domain-model.md` | Entity definitions, relationships, ERD |
| `api-spec.yml` | REST API endpoints and schemas |
| `development-guide.md` | Setup instructions, commands, troubleshooting |

#### Scenario: Project-specific content isolation

- **WHEN** a developer reviews `ai-specs/project/`
- **THEN** all content SHALL be specific to Investment Tracker (entity names, folder paths, commands)

---

### Requirement: Content extraction from mixed files

Content from existing files SHALL be extracted based on universality:

- **Universal patterns** (apply to any .NET 10 project) → `base/`
- **Project specifics** (Investment Tracker names, paths) → `project/`

#### Scenario: C# coding guidelines extraction

- **WHEN** extracting from `backend-standards.mdc`
- **THEN** C# 14 features, naming conventions, and async patterns go to `base/csharp-standards.mdc`

#### Scenario: Project structure extraction

- **WHEN** extracting from `backend-standards.mdc`
- **THEN** folder paths like `src/InvestmentTracker.Domain` go to `project/architecture.mdc`

---

### Requirement: Architecture patterns in base tier

The `base/architecture.mdc` file SHALL document:

1. **VSA (Vertical Slice Architecture)** as the preferred approach for Web APIs
2. **Clean Architecture layers** (Domain → Infra → UI dependency direction)
3. **DDD building blocks** (Entities, Value Objects, Repositories)
4. **The "Rule of Three"** for refactoring shared code

#### Scenario: Architecture guidance completeness

- **WHEN** a developer reads `base/architecture.mdc`
- **THEN** they SHALL understand when to use VSA vs Clean Architecture layers
- **AND** they SHALL have clear guidance on DDD patterns

---

### Requirement: Base README with usage instructions

The `base/README.md` file SHALL include:

1. What the base tier is for
2. How to copy it to a new project
3. What to customize after copying
4. Version/date of last update

#### Scenario: Onboarding a new project

- **WHEN** a developer reads `base/README.md`
- **THEN** they SHALL be able to set up standards for a new project in under 5 minutes

---

### Requirement: Remove original flat structure

After restructuring, the `ai-specs/specs/` directory SHALL be removed or emptied.

#### Scenario: No duplicate content

- **WHEN** the restructure is complete
- **THEN** there SHALL be no content in both `ai-specs/specs/` and the new tiers
