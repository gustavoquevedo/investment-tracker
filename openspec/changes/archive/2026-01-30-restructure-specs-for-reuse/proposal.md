## Why

The current specs in `ai-specs/specs/` mix universal .NET 10/C# 14 standards with Investment Tracker-specific details. This makes them hard to reuse as a foundation for new projects without manual extraction and cleanup.

Restructuring into two tiers (`base/` and `project/`) enables copy-paste reuse of proven standards while keeping project-specific documentation separate.

## What Changes

- **Split `ai-specs/specs/` into `ai-specs/base/` and `ai-specs/project/`**
- Extract universal C# 14, .NET 10, testing, and architecture standards into `base/`
- Move Investment Tracker-specific data model, API spec, and project structure into `project/`
- Create a `base/README.md` explaining how to use the base tier in new projects
- Update cross-references between documents

## Capabilities

### New Capabilities

- `base-standards-structure`: Defines the structure and content of the reusable `base/` tier, including what belongs there and how files should be organized

### Modified Capabilities

_None — this is a reorganization of existing content, not a change to requirements._

## Impact

- **Files created**: New structure under `ai-specs/base/` and `ai-specs/project/`
- **Files removed**: Current flat structure under `ai-specs/specs/`
- **No code changes**: This affects documentation only
- **AI agents**: Globs in `.mdc` files will need updating to reflect new paths
