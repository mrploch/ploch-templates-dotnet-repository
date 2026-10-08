# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What This Repo Is

A **GitHub template repository** for new .NET projects that come pre-wired to the `ploch-common` and `ploch-data` sibling libraries. When someone creates a new repo from this template, they rename projects/namespaces from `MyApp` to their product name. It includes CI workflows for build, test, SonarCloud, Codacy, and NuGet publishing to GitHub Packages.

## Build Commands

```bash
# Build (uses the "WithData" solution that includes all projects)
dotnet build Ploch.DotNetRepository.WithData.slnx

# Run all tests
dotnet test Ploch.DotNetRepository.WithData.slnx

# Run a single test by name
dotnet test --filter "FullyQualifiedName~ClassName.MethodName"

# Build in Release mode (enables TreatWarningsAsErrors)
dotnet build -c Release Ploch.DotNetRepository.WithData.slnx
```

There are **two solution files**:
- `Ploch.DotNetRepository.slnx` — minimal: app + tests + referenced ploch-common/ploch-data projects
- `Ploch.DotNetRepository.WithData.slnx` — full: adds all ploch-common, ploch-data, and ploch-commandline source projects for local development. **Use this one for day-to-day work.**

## Cross-Repo Dependencies

**Critical:** Sibling repos (`ploch-common`, `ploch-data`, `ploch-commandline`, `mrploch-development`) must be cloned alongside this repo under the same parent directory (`C:\DevNet\my\mrploch\`). Projects reference each other via relative paths like `../ploch-data/src/Data.Model/Ploch.Data.Model.csproj`.

Package version definitions are imported from `../mrploch-development/dependencies/` via `Directory.Packages.props` (central package management is currently **disabled** — `ManagePackageVersionsCentrally=false`).

## Architecture

Target framework: **net10.0**. All projects use `Nullable=enable` and `LangVersion=default`.

```
src/
  MyApp/                    → Console app entry point (Ploch.MyApp.csproj)
  MyApp.DomainModel/        → POCO entities implementing Ploch.Data.Model interfaces
  MyApp.Data/               → EF Core DbContext, entity configurations, DI registration
  MyApp.Data.SqLite/        → SQLite design-time factory + migrations
  MyApp.Data.SqlServer/     → SQL Server design-time factory + migrations
tests/
  MyApp.Tests/              → xUnit 3 tests using FluentAssertions, Moq, AutoFixture
```

### Domain Model Layer

Entities implement interfaces from `Ploch.Data.Model` (`IHasId<T>`, `IHasAuditProperties`, `IHasCategories<T>`, `IHasTags<T>`, etc.). Category types inherit from `Category<T>`, tag types from `Tag`. See `.claude/rules/domain-model.md` for the full pattern.

### Data Layer

`MyAppDbContext` uses assembly-scanned `IEntityTypeConfiguration<T>` classes in `Data/Configurations/`. Includes automatic audit timestamp tracking for `IHasAuditTimeProperties` entities. DI registration via `services.AddDataServices(configureOptions)` which registers both the DbContext and generic repositories from `ploch-data`.

### Provider Projects (SqLite / SqlServer)

Each contains a design-time factory inheriting from `SqLiteDbContextFactory` or `SqlServerDbContextFactory` (from `ploch-data`), plus PowerShell scripts for migration management:
- `recreate-migrations.ps1` — delete and recreate migrations
- `update-database.ps1` — apply migrations
- `recreate-migrations-update-database.ps1` — nuclear option: drop DB + recreate

### Build Configuration (Directory.Build.props)

- Auto-detects test projects by name suffix `Tests` → sets `IsPackable=false`, disables XML docs
- Non-test projects → `GeneratePackageOnBuild=true`, XML docs enabled
- Release builds → `TreatWarningsAsErrors=true`, embedded debug symbols
- Six analyzers enforced globally: StyleCop, Roslynator, SonarAnalyzer, CodeCracker, MS NetAnalyzers, VS Threading Analyzers

## Testing

Uses xUnit 3 with FluentAssertions, Moq, and AutoFixture (via `Ploch.TestingSupport.XUnit3.AutoMoq` from ploch-common). Test method naming: `MethodName_should_describe_expected_behaviour`.

## Naming Conventions

- **PascalCase** for methods and properties (C# standard, not camelCase)
- American-English spelling (`Color`, not `Colour`)
- `Id` not `ID` (e.g. `UserId`)
- Project naming: `src/<Name>/Ploch.<Product>.<Name>.csproj`
- Commit messages: Conventional Commits format (`feat:`, `fix:`, `chore:`, etc.)

## CI/CD

GitHub Actions workflow (`build-dotnet.yml`) uses the shared composite action `mrploch/ploch-github-actions/build-test-sonar@main`. Publishes NuGet packages to GitHub Packages at `nuget.pkg.github.com/mrploch`. Branch: `main`.
