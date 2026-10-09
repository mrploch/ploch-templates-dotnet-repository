#Requires -Version 7.2
<#
.SYNOPSIS
    Turns a fresh copy of this template into a named repository.

.DESCRIPTION
    Replaces the template's placeholder names in every tracked text file and in file and directory names:

        TemplateLibrary                    ->  the product part of -Name (for example "Orders" for "Ploch.Orders")
        ploch-templates-dotnet-repository  ->  -RepositoryName

    It also removes the template-only sections of README.md (between "<!-- template-only:start -->" and
    "<!-- template-only:end -->"), then deletes the template-only files: this script and the
    Template Bootstrap workflow that tests it.

    Run it once, from the repository root, straight after creating the repository with "Use this template".
    It refuses to run with uncommitted changes, so the result can be reviewed with `git diff` and undone with
    `git checkout .`. It is idempotent: a repository with no placeholders left is reported and left untouched.

.PARAMETER Name
    The product's root name, which becomes the solution, project, assembly, package and namespace prefix.
    It must start with "Ploch." and consist of PascalCase segments, for example "Ploch.Orders" or "Ploch.Text.Slugs".

.PARAMETER RepositoryName
    The GitHub repository name, used in URLs and badges. Defaults to the name of the 'origin' remote, or to
    the current directory name when there is no remote.

.PARAMETER KeepScript
    Keeps the template-only files (this script and its workflow) after a successful run.
    Intended for testing the script itself.

.EXAMPLE
    ./scripts/Initialize-Repository.ps1 -Name Ploch.Orders -WhatIf

    Lists every file the script would change or rename, without changing anything.

.EXAMPLE
    ./scripts/Initialize-Repository.ps1 -Name Ploch.Orders

    Initialises the repository as Ploch.Orders, taking the repository name from the 'origin' remote.
#>
[CmdletBinding(SupportsShouldProcess)]
param(
    [Parameter(Mandatory)]
    [ValidatePattern('^Ploch(\.[A-Z][A-Za-z0-9]*)+$')]
    [string] $Name,

    [ValidatePattern('^[a-z0-9][a-z0-9.-]*$')]
    [string] $RepositoryName,

    [switch] $KeepScript
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$ProductToken = 'TemplateLibrary'
$RepositoryToken = 'ploch-templates-dotnet-repository'
$TemplateOnlyPattern = '(?s)\r?\n?<!-- template-only:start -->.*?<!-- template-only:end -->\r?\n?'

$root = (git rev-parse --show-toplevel 2>$null)
if (-not $root) {
    throw '[Initialize-Repository] Run this script inside the git repository created from the template.'
}
Set-Location $root
$scriptPath = [System.IO.Path]::GetRelativePath($root, $PSCommandPath).Replace('\', '/')
$templateOnlyFiles = @($scriptPath, '.github/workflows/template-bootstrap.yml')

if (-not $RepositoryName) {
    $remote = git remote get-url origin 2>$null
    $RepositoryName = if ($remote) { ($remote -split '[/:]')[-1] -replace '\.git$', '' } else { Split-Path $root -Leaf }
    Write-Information "[Initialize-Repository] Repository name: $RepositoryName" -InformationAction Continue
}

if ($Name -match '(Tests|IntegrationTests|TestingSupport)$') {
    # Directory.Build.props treats projects whose names end in "Tests" as test projects, which are never packed.
    throw "[Initialize-Repository] '$Name' ends like a test project name; choose a product name such as 'Ploch.Orders'."
}

$product = $Name.Substring('Ploch.'.Length)
$replacements = [ordered]@{
    $RepositoryToken = $RepositoryName
    $ProductToken    = $product
}

$trackedFiles = @(git ls-files) | Where-Object { $_ -notin $templateOnlyFiles }
$filesWithTokens = @($trackedFiles | Where-Object {
        $content = Get-Content -LiteralPath $_ -Raw -ErrorAction SilentlyContinue
        $_ -cmatch $ProductToken -or ($content -and ($content.Contains($ProductToken) -or $content.Contains($RepositoryToken)))
    })

if ($filesWithTokens.Count -eq 0) {
    Write-Information '[Initialize-Repository] No template placeholders remain; the repository is already initialised.' -InformationAction Continue
    return
}

if (-not $WhatIfPreference -and (git status --porcelain)) {
    throw '[Initialize-Repository] The working tree has uncommitted changes. Commit or stash them first, so the result can be reviewed and undone.'
}

function Test-IsTextFile([string] $Path) {
    $bytes = [System.IO.File]::ReadAllBytes((Join-Path $root $Path))
    return -not ($bytes | Select-Object -First 8000 | Where-Object { $_ -eq 0 })
}

# 1. File contents.
foreach ($file in $trackedFiles) {
    if (-not (Test-IsTextFile $file)) {
        continue
    }

    $fullPath = Join-Path $root $file
    $bytes = [System.IO.File]::ReadAllBytes($fullPath)
    $hasBom = $bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF
    $original = [System.IO.File]::ReadAllText($fullPath)
    if ([string]::IsNullOrEmpty($original)) {
        continue
    }

    $updated = $original
    if ($file -eq 'README.md') {
        $updated = [regex]::Replace($updated, $TemplateOnlyPattern, [string]::Empty)
    }
    foreach ($token in $replacements.Keys) {
        $updated = $updated.Replace($token, $replacements[$token])
    }

    if ($updated -cne $original -and $PSCmdlet.ShouldProcess($file, 'Replace template placeholders')) {
        # Text files in the template are UTF-8; keep a byte-order mark only where the file already had one.
        # Line endings are untouched because only the placeholder text is replaced.
        [System.IO.File]::WriteAllText($fullPath, $updated, [System.Text.UTF8Encoding]::new($hasBom))
    }
}

# 2. File and directory names, deepest first so that renaming a directory never invalidates a pending path.
$pathsToRename = $trackedFiles |
    ForEach-Object {
        $segments = $_ -split '/'
        for ($i = 1; $i -le $segments.Count; $i++) {
            ($segments[0..($i - 1)] -join '/')
        }
    } |
    Where-Object { (Split-Path $_ -Leaf) -cmatch $ProductToken } |
    Sort-Object -Unique |
    Sort-Object { ($_ -split '/').Count } -Descending

foreach ($path in $pathsToRename) {
    $leaf = Split-Path $path -Leaf
    $newLeaf = $leaf.Replace($ProductToken, $product)
    $parent = Split-Path $path -Parent
    $newPath = if ($parent) { "$parent/$newLeaf" } else { $newLeaf }
    if ($PSCmdlet.ShouldProcess($path, "Rename to $newLeaf")) {
        git mv -- "$path" "$newPath"
        if ($LASTEXITCODE -ne 0) {
            throw "[Initialize-Repository] git mv failed for '$path'."
        }
    }
}

# 3. The template-only files, including this script: a repository is initialised exactly once.
if (-not $KeepScript) {
    foreach ($file in $templateOnlyFiles | Where-Object { Test-Path -LiteralPath $_ }) {
        if ($PSCmdlet.ShouldProcess($file, 'Delete template-only file')) {
            git rm --quiet -- "$file"
            if ($LASTEXITCODE -ne 0) {
                throw "[Initialize-Repository] git rm failed for '$file'; delete it manually."
            }
        }
    }
}

if (-not $WhatIfPreference) {
    Write-Information @"
[Initialize-Repository] Initialised as $Name ($RepositoryName).

Next steps:
  1. Review the changes:      git status; git diff --cached; git diff
  2. Build and test:          dotnet build -c Release; dotnet test -c Release
  3. Commit:                  git add --all; git commit -m "chore: initialise repository from template"
  4. Finish the owner setup in README.md (SonarCloud project, secrets, branch protection).
"@ -InformationAction Continue
}
