. ./build-helpers.ps1
Clear-Solution -solutionDirectory "../" -excludeWildcards "*Test*"
Build-Solution -solutionFile "../Ploch.DotNetRepository.sln" -collectCoverage $true -sonarCloudAnalysis $true -sonarCloudProjectKey "mrploch_ploch-templates-dotnet-repository" -sonarCloudOrganization "mrploch" -sonarToken $env:SONARCLOUD_TOKEN_PlochDotNetRepositoryTemplate
