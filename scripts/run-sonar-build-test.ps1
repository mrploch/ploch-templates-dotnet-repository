. ./build-helpers.ps1
Clear-Solution -solutionDirectory "../Ploch.DotNetRepository" -excludeWildcards "*Test*"
Build-Solution -solutionFile "../Ploch.DotNetRepository/Ploch.DotNetRepository.sln" -collectCoverage $true -sonarCloudAnalysis $true -sonarCloudProjectKey "mrploch_ploch-templates-dotnet-repository" -sonarCloudOrganization "mrploch" -sonarCloudToken $env:SONARCLOUD_TOKEN_PlochDotNetRepositoryTemplate
