# Universal AI Rules

## Purpose
Defines the standard format and configuration for AI coding assistants to ensure consistent rule application across different tools (Cursor, Roo Code, Claude, etc.).

## Requirements

### Requirement: Universal Rule Format

All AI development rules SHALL be stored as standard Markdown (`.md`) files in the `ai-specs/base/` and `ai-specs/project/` directories. Metadata (globs, description) MUST be preserved in YAML frontmatter.

#### Scenario: Agent compatibility
- **WHEN** an AI agent (Antigravity, Roo Code, Claude) accesses the specs
- **THEN** it encounters standard Markdown files without vendor-specific extensions (like `.mdc`)

### Requirement: Consolidated AI Rules Index

A single `ai-specs/AI_RULES.md` file SHALL be the entry point for all project rules. It SHALL contain:
1. Project Overview & Context
2. References to Universal Standards (`ai-specs/base/`)
3. References to Project Standards (`ai-specs/project/`)
4. Build & Test Commands

#### Scenario: Single Source of Truth
- **WHEN** any AI agent needs project context
- **THEN** it reads `ai-specs/AI_RULES.md` first

### Requirement: Cursor Configuration

A `.cursorrules` file SHALL be created at the repository root to configure Cursor. It MUST point to `ai-specs/AI_RULES.md` as the source of truth.

#### Scenario: Cursor rule loading
- **WHEN** a user opens a file in Cursor
- **THEN** Cursor reads `.cursorrules`
- **AND** follows the pointer to `ai-specs/AI_RULES.md`

### Requirement: Roo Code / Cline Configuration

A `.clinerules` file SHALL be created at the repository root to configure Roo Code / Cline. It MUST point to `ai-specs/AI_RULES.md` as the source of truth.

#### Scenario: Roo Code session
- **WHEN** a user starts a session with Roo Code
- **THEN** Roo Code reads `.clinerules`
- **AND** follows the pointer to `ai-specs/AI_RULES.md`

### Requirement: Claude Code Configuration

A `CLAUDE.md` file SHALL be created at the repository root. It MUST point to `ai-specs/AI_RULES.md` as the source of truth.

#### Scenario: Claude Code context
- **WHEN** a user runs `claude` in the repository
- **THEN** Claude reads `CLAUDE.md`
- **AND** follows the pointer to `ai-specs/AI_RULES.md`

### Requirement: Project Documentation Update

The `ai-specs/base/README.md` and repository `README.md` SHALL be updated to reference the new `.md` file paths and explain the multi-agent configuration via `AI_RULES.md`.

#### Scenario: Developer Onboarding
- **WHEN** a developer reads the README
- **THEN** they see instructions for configuring their preferred AI tool using the central index
