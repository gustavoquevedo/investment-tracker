# Base Standards for .NET 10 / C# 14 Projects

This folder contains reusable development standards that can be copied to any new .NET 10/C# 14 project. The files use standard Markdown (`.md`) format for compatibility with all AI coding assistants.

## What's Included

| File | Purpose |
|------|---------|
| `csharp-standards.md` | C# 14 coding guidelines, naming conventions, async patterns |
| `dotnet-tooling.md` | .NET 10 SDK features, Central Package Management, EF Core patterns |
| `architecture.md` | VSA + Clean Architecture + DDD patterns |
| `testing-standards.md` | Microsoft.Testing.Platform, xUnit v3, FluentAssertions, Moq |
| `frontend-standards.md` | React/TypeScript/Vite standards (if applicable) |
| `documentation.md` | AI specs, documentation rules, language requirements |

## How to Use in a New Project

1. **Copy this folder** to your new project:
   ```bash
   cp -r ai-specs/base/ <new-project>/ai-specs/base/
   ```

2. **Configure AI Assistants**:
   - Create a central `ai-specs/AI_RULES.md` (or similar) that indexes all rules.
   - Point your `.cursorrules`, `.clinerules`, or `CLAUDE.md` to this index file.
   - Example configuration for Cursor (`.cursorrules`):
     ```
     Read rules in ai-specs/AI_RULES.md and apply them contextually.
     ```

3. **Review and customize** globs if your project structure differs:
   - Default globs use generic patterns like `**/*.cs`
   - Adjust if you need to include/exclude specific directories

4. **Create a `project/` folder** in the new project for project-specific docs:
   - Architecture decisions specific to that project (`architecture.md`)
   - Domain model (`domain-model.md`)
   - API specification (`api-spec.yml`)
   - Development guide (`development-guide.md`)

## What to Customize

The base standards are designed to work out-of-the-box, but you may want to:

- **Adjust technology versions** if targeting a different .NET version
- **Modify architecture patterns** if your project has different needs
- **Remove frontend standards** if it's a backend-only project

## Version

Last updated: 2026-01-30
