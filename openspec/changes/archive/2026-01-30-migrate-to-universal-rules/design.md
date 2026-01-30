## Context

Currently, AI development rules are stored as `.mdc` files in `ai-specs/base/` and `ai-specs/project/`. The `.mdc` extension is specific to Cursor's "Repo Rules" feature, which automatically loads context based on globs in YAML frontmatter.

The user wants to support other AI agents (Antigravity, Roo Code, Claude Code) which may not support `.mdc` natively. A universal format based on standard Markdown (`.md`) is required.

## Goals / Non-Goals

**Goals:**
- Convert all `.mdc` files to standard `.md` files
- Enable rule discovery for multiple agents:
    - **Cursor**: Via `.cursorrules`
    - **Roo Code / Cline**: Via `.clinerules`
    - **Claude Code**: Via `CLAUDE.md` or similar standard
- Preserve existing rule logic (globs, descriptions) in a way that remains readable
- Make rules "agent-agnostic"

**Non-Goals:**
- Changing the content of the rules themselves (beyond format)
- Supporting every possible AI tool (focus on the requested ones)

## Decisions

### 1. File Format: Standard Markdown (`.md`)

**Decision:** Rename all `.mdc` files to `.md`.

**Rationale:**
- `.md` is the universal documentation format understood by all LLMs.
- Removes vendor lock-in to Cursor.
- Still renders correctly in all IDEs and Git platforms.

### 2. Metadata Handling

**Decision:** Keep YAML frontmatter but standardize keys.

- `description`: Keep. Useful for all agents.
- `globs`: Keep. Agents can parse this to know when to apply rules.
- `alwaysApply`: Keep. Useful signal.

**Alternative:** Move metadata to a central `rules.json`.
**Reason Rejected:** Harder for LLMs to maintain context if rules are split from their definition. Self-contained files are better.

### 3. Agent Configuration Strategy

**Decision:** Use root-level configuration files to point to `ai-specs/`.

| Agent | Config File | Strategy |
|-------|-------------|----------|
| **Cursor** | `.cursorrules` | Add instruction: "Read rules in `ai-specs/base/*.md` and apply based on globs." |
| **Roo Code** | `.clinerules` | Add instruction: "Read rules in `ai-specs/base/*.md` and apply based on globs." |
| **Claude** | `CLAUDE.md` | Create file summarizing project context and pointing to `ai-specs/`. |

**Rationale:** Each tool has its own entry point. We configure the entry point to look at our shared "Source of Truth" in `ai-specs/`.

### 4. Updates to Existing Documents

**Decision:** Update `ai-specs/base/README.md` to reflect the new structure and explain how to configure agents.

## Risks / Trade-offs

| Risk | Mitigation |
|------|------------|
| **Cursor Automation Lost** | `.mdc` files are auto-loaded by Cursor. `.md` files are not unless referenced. We mitigate this by adding a `.cursorrules` file that explicitly instructs the model to check `ai-specs/`. |
| **Agent Confusion** | If an agent doesn't support glob parsing from markdown, it might apply rules incorrectly. We rely on the agent's intelligence to interpret the written instruction "Apply this to *.cs files". |

## Migration Plan

1. Rename `ai-specs/base/*.mdc` -> `*.md`
2. Rename `ai-specs/project/*.mdc` -> `*.md`
3. Create `.cursorrules`
4. Create `.clinerules`
5. Create `CLAUDE.md`
6. Update `ai-specs/base/README.md`
