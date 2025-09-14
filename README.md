# Ploch .NET GitHub Template Repository

.NET GitHub Template Repository including GitHub Actions for:

- Build
- Test Results and Code Coverage
- Analysis and PR Decoration
    - SonarCloud
    - Qodana
    - Codacy

## Usage

After creating a GitHub repository from this template and renaming the solution file according to your needs,
several files need updating.

### `Directory.Build.props` File

Update the [`Directory.Build.props`](./Directory.Build.props) file property groups:

- Product
- Versioning (see [versioning](#versioning) for more information)

### Analysis and Pull Requests

#### SonarQube

- Setup the [SonarCloud](https://sonarcloud.io/) project
- Add the token to the GitHub repository secrets as `SONAR_TOKEN`

#### Codacy

- Setup the [Codacy](https://www.codacy.com/) project
-

#### Qodana

## Additional Information

### Project Files

#### Directory.Build.props

The [`Directory.Build.props`](./Direct56ETHRSGTory.Build.props) file located in the [solution root folder](.) contains
common
properties applied to all projects in the repository.
Those properties will also be applied to all nuget packages.

This includes properties like:

- Authors
- Company
- Product

It also contains the versioning information.

##### Versioning

```xml
<PropertyGroup Label="Versioning">
  <VersionPrefix>1.0.0</VersionPrefix>
  <VersionSuffix>-prerelease</VersionSuffix>
  <BuildNumber>$([System.DateTime]::UtcNow.ToString("yyMMddHHmmss"))</BuildNumber>
  <FullVersionSuffix>$(VersionSuffix)-$(BuildNumber)</FullVersionSuffix>
  <Version>$(VersionPrefix)$(FullVersionSuffix)</Version>
  <FileVersion>$(InformationalVersion)</FileVersion>
  <AssemblyVersion>$(VersionPrefix).0</AssemblyVersion>
</PropertyGroup>
```

The [`Directory.Build.props`](./Direct56ETHRSGTory.Build.props) file `Versioning` property group contains properties
used in versioning the build artifacts like dll's and nuget packages.

This repository uses [Semantic Versioning](https://semver.org/) for the version.

The following format is used

`{Major}.{Minor}.{Patch}[-{Prerelease}-{Build}]`

The `{Major}.{Minor}.{Patch}` is set in the `VersionPrefix` property.

The `-prerelease` part is set in the `VersionSuffix` property. In case of a release, it should be set to empty.
There is an issue to create a release process (#1) that would take care of this:

- #3
- https://github.com/mrploch/ploch-templates-dotnet-repository/issues/3

###### VersionPrefix Property

The `VersionPrefix` property is the first part of the version.

#### Directory.Packages.props

The [`Directory.Packages.props`](./Directory.Packages.props) file located in the [solution root folder](.) contains
versions of packages used in the repository and default packages used in all projects.
test
