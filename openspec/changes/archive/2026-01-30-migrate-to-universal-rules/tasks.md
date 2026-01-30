## 1. Convert Files to Standard Markdown

- [x] 1.1 Rename `ai-specs/base/*.mdc` files to `*.md`
- [x] 1.2 Rename `ai-specs/project/*.mdc` files to `*.md`
- [x] 1.3 Verify file content is intact (metadata frontmatter preserved)

## 2. Configure AI Tools

- [x] 2.1 Create `.cursorrules` pointing to `ai-specs/` directory
- [x] 2.2 Create `.clinerules` pointing to `ai-specs/` directory
- [x] 2.3 Create `CLAUDE.md` with project summary and pointers to `ai-specs/`
- [x] 2.4 Consolidated AI Rules: Create `ai-specs/AI_RULES.md` and update configs to point to it

## 3. Update Existing Documentation

- [x] 3.1 Update `ai-specs/base/README.md` to reference `.md` files and tool usage
- [x] 3.2 Update root `README.md` to reflect new standard

## 4. Verification

- [x] 4.1 Verify Cursor loads rules (manual check)
- [x] 4.2 Verify Roo Code / Antigravity sees rules (manual check)
- [x] 4.3 Verify no broken links in documentation
