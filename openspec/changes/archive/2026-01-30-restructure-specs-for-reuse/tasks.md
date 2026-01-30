## 1. Create Directory Structure

- [x] 1.1 Create `ai-specs/base/` directory
- [x] 1.2 Create `ai-specs/project/` directory

## 2. Create Base Tier Files

- [x] 2.1 Create `base/README.md` with usage instructions
- [x] 2.2 Create `base/csharp-standards.mdc` - extract C# 14 guidelines, naming, async patterns from `backend-standards.mdc`
- [x] 2.3 Create `base/dotnet-tooling.mdc` - extract .NET 10 SDK, CPM, .slnx, EF Core patterns from `backend-standards.mdc` and `development_guide.md`
- [x] 2.4 Create `base/architecture.mdc` - extract VSA, Clean Architecture, DDD patterns from `backend-standards.mdc`
- [x] 2.5 Create `base/testing-standards.mdc` - extract MTP, xUnit v3, testing patterns from `backend-standards.mdc`
- [x] 2.6 Copy `frontend-standards.mdc` to `base/frontend-standards.mdc` (adjust globs)
- [x] 2.7 Create `base/documentation.mdc` - combine content from `base-standards.mdc` and `documentation-standards.mdc`

## 3. Create Project Tier Files

- [x] 3.1 Create `project/architecture.mdc` - project-specific layer mapping, folder structure
- [x] 3.2 Move `data-model.md` to `project/domain-model.md`
- [x] 3.3 Move `api-spec.yml` to `project/api-spec.yml`
- [x] 3.4 Create `project/development-guide.md` - project-specific setup, commands, troubleshooting

## 4. Update Globs and References

- [x] 4.1 Update globs in base `.mdc` files to use generic patterns (`**/*.cs`)
- [x] 4.2 Update globs in project `.mdc` files to use project-specific paths
- [x] 4.3 Update cross-references between documents

## 5. Cleanup

- [x] 5.1 Remove or archive original `ai-specs/specs/` directory
- [x] 5.2 Verify no duplicate content exists

## 6. Verification

- [x] 6.1 Test AI agent pickup of new structure with a sample prompt
- [x] 6.2 Review all files for completeness and consistency
