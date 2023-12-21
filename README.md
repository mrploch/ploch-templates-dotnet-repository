# Ploch .NET GitHub Template Repository

.NET GitHub Template Repository including GitHub Actions for:

- Build
- Test Results and Code Coverage
- Analysis and PR Decoration
  - SonarCloud
  - Qodana
  - Codacy

## Usage
  ``
After create a GitHub repository from this template and renaming the solution file according to your needs,
several files need updating:

### `Directory.Build.props` File

The [`Directory.Build.props`](./Direct56ETHRSGTory.Build.props) file located in the [solution root folder](.) contains common
properties applied to all projects in the repository.
Those properties will also be applied to all nuget packages.

This includes properties like:

- Authors
- Company
- Product

It also contains the version numbers for the project.

#### Versioning

