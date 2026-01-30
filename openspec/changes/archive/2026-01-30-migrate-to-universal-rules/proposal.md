## Why

The current AI coding standards rely on `.mdc` files, which are specific to Cursor (Cursor Rules). To support a wider range of AI coding assistants (Antigravity, Roo Code, Claude Code, GitHub Copilot), we need a universal format that isn't tied to a single IDE's proprietary extension.

## What Changes

- **Convert `.mdc` files to standard Markdown (`.md`)**: Rename all files in `ai-specs/base/` and `ai-specs/project/`.
- **Standardize Metadata**: Move rule metadata (description, globs, triggers) from YAML frontmatter to a standardized section in the Markdown body or a central index file.
- **Create Universal Entry Points**: Add configuration files for major AI tools that point to the shared `ai-specs/` directory:
    - `.cursorrules` (for Cursor)
    - `.clinerules` (for Roo Code / Cline)
    - `.github/copilot-instructions.md` (for GitHub Copilot)
- **Centralize Rule Index**: Create a `project-rules.md` or similar index that acts as a map for agents to discover available standards.

## Capabilities

### New Capabilities

- `universal-ai-rules`: Defines the standard for rule definition, discovery, and cross-tool compatibility.
- `rule-conversion`: Defines the process for converting proprietary formats (like `.mdc`) to the universal standard.

### Modified Capabilities

- `base-standards-structure`: Update the `base` tier definition to require `.md` files instead of `.mdc`.

## Impact

- **File System**: Renaming ~11 files in `ai-specs/`.
- **Tool Configuration**: New config files at root.
- **Agent Behavior**: Agents will need to be pointed to the new entry points.
- **Maintenance**: Rule updates now happen in standard Markdown, simplifying editing.
