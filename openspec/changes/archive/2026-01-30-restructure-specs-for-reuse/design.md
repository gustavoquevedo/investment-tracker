## Context

The `ai-specs/specs/` folder contains 7 files mixing universal .NET 10/C# 14 standards with Investment Tracker-specific content:

| File | Content Type |
|------|--------------|
| `backend-standards.mdc` | Mixed (C# guidelines + project structure) |
| `base-standards.mdc` | Mostly universal |
| `documentation-standards.mdc` | Universal |
| `frontend-standards.mdc` | Universal |
| `development_guide.md` | Mixed (general setup + project paths) |
| `api-spec.yml` | Project-specific |
| `data-model.md` | Project-specific |

This change restructures into two tiers for clean reuse.

## Goals / Non-Goals

**Goals:**
- Create a `base/` tier that can be copied to any new .NET 10/C# 14 project
- Keep project-specific documentation in `project/` tier
- Maintain all existing content (no information loss)
- Update globs in `.mdc` files to match new paths

**Non-Goals:**
- Rewriting or improving the content itself (that's a separate change)
- Adding new standards not already documented
- Changing the spec format or frontmatter structure

## Decisions

### 1. Directory structure: `ai-specs/base/` and `ai-specs/project/`

**Decision:** Create two sibling directories under `ai-specs/`.

**Alternatives considered:**
- Separate repositories for base standards → Too much overhead for this use case
- Symlinks → Platform compatibility issues on Windows

**Rationale:** Simple, explicit, works everywhere.

### 2. Base tier file organization

**Decision:** Organize by concern, not by original filename:

```
base/
├── README.md                 # How to use this base in new projects
├── csharp-standards.mdc      # C# 14 coding guidelines, naming, async
├── dotnet-tooling.mdc        # .NET 10 SDK, CPM, .slnx, EF Core patterns
├── architecture.mdc          # VSA + Clean Architecture + DDD patterns
├── testing-standards.mdc     # MTP, xUnit v3, FluentAssertions, Moq
├── frontend-standards.mdc    # React/TypeScript/Vite (copy as-is)
└── documentation.mdc         # AI specs, commit docs, language rules
```

**Rationale:** Logical grouping makes it easier to find and update specific topics.

### 3. Project tier file organization

**Decision:** Keep project-specific files with minimal changes:

```
project/
├── architecture.mdc          # Project-specific layer mapping
├── domain-model.md           # Entity definitions, ERD
├── api-spec.yml              # REST endpoints
└── development-guide.md      # Setup, commands, troubleshooting
```

**Rationale:** These are specific to Investment Tracker and will be replaced entirely in new projects.

### 4. Content extraction from mixed files

**Decision:** Extract content based on this principle:
- **Universal patterns** (would apply to any .NET 10 project) → `base/`
- **Investment Tracker specifics** (folder paths, entity names) → `project/`

Examples:
| Content | Destination |
|---------|-------------|
| "Use `record` for DTOs" | `base/csharp-standards.mdc` |
| "Domain layer in `src/InvestmentTracker.Domain`" | `project/architecture.mdc` |
| "VSA for Web APIs, Clean Arch for domain" | `base/architecture.mdc` |
| "Asset, Snapshot, Contribution entities" | `project/domain-model.md` |

### 5. Glob updates in `.mdc` files

**Decision:** Update globs to use relative paths from new locations.

Example for `base/csharp-standards.mdc`:
```yaml
globs: ["**/*.cs"]  # Applies to all C# files in any project
```

Example for `project/architecture.mdc`:
```yaml
globs: ["src/**/*.cs", "tests/**/*.cs"]  # Specific to this project structure
```

## Risks / Trade-offs

| Risk | Mitigation |
|------|------------|
| AI agents might not pick up the new structure | Test with a simple prompt after restructure |
| Some content is genuinely mixed | Make a judgment call; err toward `base/` if reusable |
| Maintenance burden of two tiers | Clear separation actually reduces confusion |

## Open Questions

_None — the scope is well-defined as a documentation reorganization._
