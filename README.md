# Ploch.TemplateLibrary

[![Build](https://github.com/mrploch/ploch-templates-dotnet-repository/actions/workflows/build.yml/badge.svg)](https://github.com/mrploch/ploch-templates-dotnet-repository/actions/workflows/build.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=mrploch_ploch-templates-dotnet-repository&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=mrploch_ploch-templates-dotnet-repository)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=mrploch_ploch-templates-dotnet-repository&metric=coverage)](https://sonarcloud.io/summary/new_code?id=mrploch_ploch-templates-dotnet-repository)
[![Licence](https://img.shields.io/github/license/mrploch/ploch-templates-dotnet-repository)](LICENSE)

<!-- template-only:start -->
> [!NOTE]
> **This is a GitHub template repository** for a .NET library. Everything between the `template-only` markers in
> this file is removed when a new repository is initialised; the rest becomes the new repository's README.

## Using this template

1. Create a repository from this template: **Use this template → Create a new repository** on GitHub, or
   `gh repo create mrploch/<name> --public --template mrploch/ploch-templates-dotnet-repository --clone`.
2. Initialise it, then review, build and commit the result:

   ```bash
   pwsh ./scripts/Initialize-Repository.ps1 -Name Ploch.<Product> -WhatIf   # preview
   pwsh ./scripts/Initialize-Repository.ps1 -Name Ploch.<Product>
   dotnet build -c Release && dotnet test -c Release
   git add --all && git commit -m "chore: initialise repository from template"
   ```

   The script renames the solution, projects, namespaces and URLs from `Ploch.TemplateLibrary` and
   `ploch-templates-dotnet-repository`, removes these template-only sections, then deletes itself and the
   Template Bootstrap workflow that tests it.
3. Complete the [owner setup](#owner-setup) below, then replace `TextNormalizer` with the library's real code.

## What the template provides

| Area | What you get |
|---|---|
| Build | .NET SDK pinned in `global.json`, central package management, analysers (.NET, Roslynator, SonarAnalyzer, StyleCop, VS Threading) with warnings as errors in Release |
| Tests | xUnit v3, FluentAssertions and AutoFixture; coverage through coverlet in every test project |
| CI | `build.yml` on every push to `main` and every pull request: build, test, coverage, SonarCloud; actions pinned to commit SHAs with least-privilege permissions |
| Coverage | A coverage table in the job summary and one sticky pull-request comment, with the changed lines' coverage and where tests are missing; changed lines must be 80% covered |
| Code quality | SonarCloud analysis with pull-request decoration; the build fails when the new-code quality gate fails |
| Results | A "Test Results" check, published for fork and Dependabot pull requests as well |
| Release | `release.yml`: dry run by default, NuGet trusted publishing (no API key), GitHub release notes from `change-log/` |
| Versioning | Nerdbank.GitVersioning (`version.json`), SourceLink and symbol packages |
| Hygiene | Dependabot (NuGet, SDK, Actions), issue forms, PR template, CODEOWNERS, CONTRIBUTING, SECURITY, agent instructions |

## Keeping the template current

The shared files (workflows, build properties, analyser settings) are maintained canonically in
[mrploch-development](https://github.com/mrploch/mrploch-development) and synced into the template repositories.
Repositories created from the template are independent copies: to adopt a later template change, compare the
file with the template and copy the change.
<!-- template-only:end -->

## Getting started

```bash
dotnet tool restore
dotnet build -c Release
dotnet test -c Release
```

The SDK version is pinned in [`global.json`](global.json). Release builds treat analyser warnings as errors,
exactly as CI does.

## Continuous integration

| Workflow | Runs on | Does |
|---|---|---|
| [Build](.github/workflows/build.yml) | push to `main`, pull requests, manual | Builds and tests in Release, measures coverage, runs SonarCloud |
| [PR Report](.github/workflows/pr-report.yml) | after every Build run | Publishes the "Test Results" check and the sticky coverage comment |
| [Release](.github/workflows/release.yml) | manual | Packs and publishes to nuget.org (dry run by default) |

### Code coverage

Coverage is collected by coverlet in every test project and reported in three places that use the same
exclusions, so they agree:

- **The job summary of every Build run**: overall line and branch coverage, with a per-assembly and per-class table.
- **One comment on each pull request**, updated in place on every push: coverage of the lines the pull request
  changes, listing each file's uncovered lines, followed by the overall summary. Pull requests must cover at
  least **80%** of their changed lines; set the `DIFF_COVERAGE_THRESHOLD` repository variable to change that.
- **SonarCloud**, whose quality gate checks coverage of new code.

The full HTML report is attached to every Build run as the `coverage-report` artefact. Files excluded from
coverage (generated code and migrations) are listed once, in `COVERAGE_EXCLUDE` in `build.yml`.

## Owner setup

These steps need repository or organisation administrator access and cannot be automated from the repository.

### SonarCloud

1. In [SonarCloud](https://sonarcloud.io/projects/create), choose **Analyze new project**, select the `mrploch`
   organisation and this repository. The project key must be `mrploch_ploch-templates-dotnet-repository`
   (or set the `SONAR_PROJECT_KEY` repository variable to the key you chose).
2. In the project's **Administration → Analysis Method**, turn **Automatic Analysis off**. CI-based analysis fails
   while it is on.
3. In **Administration → New Code**, select **Previous version**.
4. Provide a token as the `SONAR_TOKEN` secret: an organisation-level secret is inherited by every repository;
   otherwise generate one under **My Account → Security** and add it under **Settings → Secrets and variables →
   Actions**. The organisation defaults to the repository owner; set the `SONAR_ORGANIZATION` variable to override it.

Until `SONAR_TOKEN` exists and the SonarCloud project has been created, the Build workflow skips SonarCloud with
a warning rather than failing, so a new repository is green from its first push. A token that exists but is
rejected by SonarCloud fails the build.

### Security reporting

Under **Settings → Code security**, enable **Private vulnerability reporting**. `SECURITY.md` and the issue-form
contact link send reporters there; with it disabled, they have no private channel.

### Branch protection

Protect `main` and require the **Build, test and analyse** and **Test Results** checks. Do not require the
external **SonarCloud Code Analysis** check: fork and Dependabot pull requests skip SonarCloud, so they would never
report it. The SonarCloud quality gate is enforced inside **Build, test and analyse** instead, which waits for the
gate and fails when it fails.

### Releasing

1. On [nuget.org](https://www.nuget.org/account/trustedpublishing), add a trusted publishing policy for owner
   `mrploch`, repository `ploch-templates-dotnet-repository` and workflow file `release.yml`.
2. Set the `NUGET_USER` repository variable to the nuget.org account that owns the policy.
3. Under **Settings → Actions → General**, allow GitHub Actions to create pull requests. The release opens a
   bookkeeping pull request with the workflow token, and GitHub does not start workflows for events caused by
   that token: close and reopen the pull request to run its checks before merging.
4. Run the **Release** workflow with `dry_run` ticked, then again without it. The version is `major.minor`
   (for example `1.0`); Nerdbank.GitVersioning adds the patch number.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md). Report security issues privately as described in [SECURITY.md](SECURITY.md).

## Licence

[Apache 2.0](LICENSE)
