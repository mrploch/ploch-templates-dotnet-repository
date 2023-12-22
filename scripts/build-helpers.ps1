<#
.SYNOPSIS
Restores, builds and tests solution with an option to run SonarCloud analysis

.DESCRIPTION
The Build-Solution function restores and builds the .NET solution. Additionally, it can run unit tests with coverage and SonarCloud analysis if required parameters are provided

.PARAMETER solutionFile
Path to the solution file

.PARAMETER collectCoverage
Boolean indicating whether code coverage should be collected

.PARAMETER sonarCloudAnalysis
Boolean indicating whether to perform SonarCloud analysis

.PARAMETER sonarCloudProjectKey
The project key for the SonarCloud analysis

.PARAMETER sonarCloudOrganization
The organization name for the SonarCloud analysis

.PARAMETER sonarCloudToken
The token for the SonarCloud analysis

.EXAMPLE
Build-Solution -solutionFile "./MyApp/myApp.csproj" -collectCoverage $true -sonarCloudAnalysis $true -sonarCloudProjectKey "project1" -sonarCloudOrganization "org1" -sonarCloudToken "abc123"
#>
function Build-Solution([Parameter(Mandatory = $true)][string] $solutionFile,
                        [Parameter(Mandatory = $false)][bool] $collectCoverage = $false,
                        [Parameter(Mandatory = $false)][bool] $sonarCloudAnalysis = $false,
                        [Parameter(Mandatory = $false)][string] $sonarCloudProjectKey,
                        [Parameter(Mandatory = $false)][string] $sonarCloudOrganization,
                        [Parameter(Mandatory = $false)][string] $sonarToken)
{
    $InformationPreference = 'Continue'
    $WarningPreference = 'Continue'
    $DebugPreference = 'Continue'
    $VerbosePreference = 'Continue'
    Write-Information 'Build-Solution:'
    Write-Information "`$solutionFile = '$solutionFile'"
    Write-Information "`$collectCoverage = $collectCoverage"
    Write-Information "`$sonarCloudAnalysis = $sonarCloudAnalysis"
    Write-Information "`$sonarCloudProjectKey = '$sonarCloudProjectKey'"
    Write-Information "`$sonarCloudOrganization = '$sonarCloudOrganization'"
    Write-Information "sonarToken = $sonarToken"
    $solutionFileItem = Get-Item $solutionFile
    $solutionDirectory = $solutionFileItem.DirectoryName
    
    Write-Information "solutionDirectory: $solutionDirectory"
    $currentDir = $PSScriptRoot
    set-location $solutionFileItem.DirectoryName
    if ($sonarCloudAnalysis)
    {
        if ((-not$sonarCloudProjectKey) -or (-not$sonarCloudOrganization) -or (-not$sonarToken))
        {
            throw 'sonarCloudProjectKey is required when sonarCloudAnalysis is true'
        }
        Write-Information 'Running with the SonarQube analysis'
        Write-Information "sonarCloudProjectKey: $sonarCloudProjectKey, sonarCloudOrganization: $sonarCloudOrganization, sonarToken: $sonarToken"
        Write-Information 'Installing dotnet-sonarscanner tool'
        dotnet tool install --global dotnet-sonarscanner
    }

    dotnet restore $solutionFileItem.FullName

    if ($sonarCloudAnalysis)
    {
        Write-Information 'dotnet sonarscanner begin /k:"$sonarCloudProjectKey" /o:"$sonarCloudOrganization" /d:sonar.login = "$sonarToken" /d:sonar.cs.opencover.reportsPaths = **/CoverageResults/coverage.opencover.xml /d:sonar.host.url = "https://sonarcloud.io"'
        dotnet sonarscanner begin /k:"$sonarCloudProjectKey" /o:"$sonarCloudOrganization" /d:sonar.login="$sonarToken" /d:sonar.cs.opencover.reportsPaths=**/CoverageResults/coverage.opencover.xml /d:sonar.host.url="https://sonarcloud.io"
    }
    dotnet build $solutionFileItem.FullName --no-incremental --no-restore
    if ($collectCoverage)
    {
        dotnet test $solutionFileItem.FullName --verbosity normal --no-build --logger "trx;LogFileName=TestOutputResults.xml" /p:CollectCoverage = true /p:CoverletOutput = ./CoverageResults/ "/p:CoverletOutputFormat=cobertura%2copencover"
    }
    if ($sonarCloudAnalysis)
    {
        dotnet sonarscanner end /d:sonar.login="$sonarToken"
    }
    
    cd $currentDir
}

<#
.SYNOPSIS
Clears solution directory

.DESCRIPTION
The Clear-Solution function removes certain folders from the provided solution directory.

.PARAMETER solutionDirectory
The directory of the solution

.PARAMETER excludeWildcards
Array of wildcards to exclude from the deletion

.EXAMPLE
Clear-Solution -solutionDirectory "./MyApp" -excludeWildcards "*Test*"
#>
function Clear-Solution([Parameter(Mandatory = $true)] [string] $solutionDirectory, [string[]] $excludeWildcards = @())
{
    Write-Output "solutionDirectory: $solutionDirectory, excludeWildcards: $excludeWildcards"
    if ((Get-Item -Path $solutionDirectory) -isnot [System.IO.DirectoryInfo])
    {
        $solutionDirectory = [System.IO.Path]::GetDirectoryName($solutionDirectory)
    }
    Remove-Folders -path $solutionDirectory -folderNames obj, bin, CoverageResults -excludeWildcards $excludeWildcards
}

# Helper functions

function Get-FolderName([Parameter(Mandatory = $true)] [string] $path)
{
    if ((Get-Item -Path $solutionDirectory) -is [System.IO.DirectoryInfo])
    {
        return $path
    }
    return [System.IO.Path]::GetDirectoryName($solutionDirectory)
}

function Remove-Folders([Parameter(Mandatory = $true)] [string] $path, [Parameter(Mandatory = $true)] [string[]] $folderNames, [string[]] $excludeWildcards = @())
{
    $items = get-childitem -Path $path -Recurse -force -Directory -Include $folderNames
    foreach ($item in $items)
    {
        $remove = $true
        foreach ($excludeWildcard in $excludeWildcards)
        {
            if ($item.FullName -like $excludeWildcard)
            {
                $remove = $false
                break
            }
        }
        if ($remove)
        {
            Write-Output $item.FullName
            Remove-Item -Path $item.FullName -Recurse -Force
        }
    }
}

