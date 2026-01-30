---
description: Documentation standards, AI specs guidelines, and language requirements for technical artifacts.
globs: ["**/*.md", "**/*.mdc", "**/README*"]
alwaysApply: true
---

# Documentation Standards

## Core Principles

- **Small tasks, one at a time**: Always work in baby steps, one at a time. Never go forward more than one step.
- **Test-Driven Development**: Start with failing tests for any new functionality (TDD)
- **Clear Naming**: Use clear, descriptive names for all variables and functions
- **Incremental Changes**: Prefer incremental, focused changes over large, complex modifications
- **Question Assumptions**: Always question assumptions and inferences
- **Pattern Detection**: Detect and highlight repeated code patterns

## Language Standards

**English Only**: All technical artifacts must always use English, including:

- Code (variables, functions, classes, comments, error messages, log messages)
- Documentation (README, guides, API docs)
- Jira tickets (titles, descriptions, comments)
- Data schemas and database names
- Configuration files and scripts
- Git commit messages
- Test names and descriptions

## Technical Documentation

Before making any commit or git push, review which technical documentation should be updated:

1. Review all recent changes in the codebase
2. Identify which documentation files need updates based on the changes:
   - For data model changes: Update domain model documentation
   - For API changes: Update API specification
   - For library/migration changes: Update standards files
3. Update each affected documentation file in English
4. Ensure all documentation is properly formatted and follows established structure
5. Verify that all changes are accurately reflected in the documentation

## AI Specs (Standards for AI Agents)

### Learning from Feedback

AI agents must:

- Learn from user feedback, guidance, and suggestions during interactions
- Identify opportunities to improve existing Development Rules proactively
- Keep assistance aligned with evolving project needs and user expectations
- Incorporate user feedback into the operational framework

### Common Pitfalls to Avoid

- **Skipping Approval Process**: Applying rule modifications without user review
- **Unlinked Proposals**: Proposing changes without connecting to specific feedback
- **Imprecise Modifications**: Suggesting changes without identifying which rule should change
- **Unaddressed Feedback**: Not initiating learning when relevant feedback is provided
- **Scope Creep**: Updating multiple unrelated rules simultaneously
- **Unprompted Rule Changes**: Modifying rules without connection to user feedback
- **Missing Update Confirmation**: Failing to notify after rule modification

## Git Commit Messages

Follow conventional commit format:

```
<type>(<scope>): <description>

[optional body]

[optional footer]
```

Types: `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`
