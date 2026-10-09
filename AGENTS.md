# Agent instructions

Instructions for AI coding agents working in this repository. Human contributors: see
[CONTRIBUTING.md](CONTRIBUTING.md).

## Repository

- .NET library, `Ploch.TemplateLibrary`, targeting the SDK pinned in `global.json`.
- `src/` holds shipping projects and `tests/` their tests (xUnit v3, FluentAssertions, AutoFixture).
- Package versions are pinned centrally in `Directory.Packages.props`; never put a version in a `.csproj`.
- Shared MSBuild settings live in `Directory.Build.props`. Versions come from `version.json` (Nerdbank.GitVersioning).

## Commands

```bash
dotnet tool restore
dotnet build Ploch.TemplateLibrary.slnx -c Release
dotnet test Ploch.TemplateLibrary.slnx -c Release
```

## Rules

- Release builds treat warnings as errors. Fix analyser warnings; do not suppress them without a written reason.
- Every public type and member needs XML documentation.
- Test methods are named `<Member>_should_<expected_behaviour>`; test classes `<TypeUnderTest>Tests`.
- New and changed code needs tests: CI requires 80% coverage of changed lines, and SonarCloud gates new code.
- Commits follow Conventional Commits. Add a `change-log/` entry for every user-visible change.
- Never commit credentials. Secrets reach CI only through GitHub secrets; see `README.md`.
