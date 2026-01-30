## ADDED Requirements

### Requirement: Universal Rule Format

All AI development rules SHALL be stored as standard Markdown (`.md`) files in the `ai-specs/base/` and `ai-specs/project/` directories. Metadata (globs, description) MUST be preserved in YAML frontmatter.

#### Scenario: Agent compatibility
- **WHEN** an AI agent (Antigravity, Roo Code, Claude) accesses the specs
- **THEN** it encounters standard Markdown files without vendor-specific extensions (like `.mdc`)

### Requirement: Cursor Configuration

A `.cursorrules` file SHALL be created at the repository root to configure Cursor to use the universal rules. It MUST explicitly instruct Cursor to read rules from `ai-specs/base/*.md` and `ai-specs/project/*.md` and respect their glob patterns.

#### Scenario: Cursor rule loading
- **WHEN** a user opens a file in Cursor
- **THEN** Cursor reads `.cursorrules`
- **AND** loads the relevant rules from `ai-specs/` based on file context

### Requirement: Roo Code / Cline Configuration

A `.clinerules` file SHALL be created at the repository root to configure Roo Code / Cline. It MUST explicitly instruct the agent to use the rules in `ai-specs/`.

#### Scenario: Roo Code session
- **WHEN** a user starts a session with Roo Code
- **THEN** Roo Code reads `.clinerules`
- **AND** understands where to find the project standards

### Requirement: Claude Code Configuration

A `CLAUDE.md` file SHALL be created at the repository root. It MUST summarize the project context (build commands, architecture) and point to the detailed standards in `ai-specs/`.

#### Scenario: Claude Code context
- **WHEN** a user runs `claude` in the repository
- **THEN** Claude reads `CLAUDE.md` for immediate context
- **AND** knows where to find deeper technical standards

### Requirement: Project Documentation Update

The `ai-specs/base/README.md` and repository `README.md` SHALL be updated to reference the new `.md` file paths and explain the multi-agent configuration.

#### Scenario: Developer Onboarding
- **WHEN** a developer reads the README
- **THEN** they see instructions for configuring their preferred AI tool (Cursor, Roo, etc.)
