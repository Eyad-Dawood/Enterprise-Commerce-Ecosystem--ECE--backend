<#
.SYNOPSIS
    Renames the project from "MyProject" to a custom name.
.DESCRIPTION
    This script recursively replaces all occurrences of "MyProject" within source files
    and renames files and directories to match the new project name.
    It follows the requirements outlined in README.md but uses a robust recursive approach.
#>

param(
    [Parameter(Mandatory=$false)]
    [string]$NewProjectName
)

# 1. Prompt for new name if not provided
if ([string]::IsNullOrWhiteSpace($NewProjectName)) {
    $NewProjectName = Read-Host "Enter the new project name (e.g., MySuperApp)"
}

if ([string]::IsNullOrWhiteSpace($NewProjectName)) {
    Write-Host "Error: Project name cannot be empty." -ForegroundColor Red
    exit 1
}

$OldName = "MyProject"
$CurrentDir = $PSScriptRoot
if ([string]::IsNullOrEmpty($CurrentDir)) { $CurrentDir = Get-Location }

Write-Host "`nRenaming project from '$OldName' to '$NewProjectName'..." -ForegroundColor Cyan

# 2. Define files to process for content replacement
# We include text-based files and exclude binaries and common build/git folders.
$IncludeExtensions = @(
    "*.sln",
    "*.csproj",
    "*.cs",
    "*.json",
    "*.yml",
    "*.yaml",
    "*.sh",
    "*.html",
    "*.htm",
    "*.css",
    "*.js",
    "*.ts",
    "*.tsx",
    "*.jsx",
    "*.props",
    "*.xml",
    "*.config",
    "*.md",
    "Dockerfile",
    ".env"
)
$ExcludePaths = @(
    ".git",
    "bin",
    "obj",
    "node_modules"
)
Write-Host "Updating file contents..." -ForegroundColor Gray
$filesToUpdate = Get-ChildItem -Path $CurrentDir -Recurse -Include $IncludeExtensions | Where-Object {
    $fullName = $_.FullName
    $skip = $false
    foreach ($p in $ExcludePaths) {
    if ($fullName -match "\\$([regex]::Escape($p))\\") {
        $skip = $true
        break
    }
}
    $skip -eq $false
}

foreach ($file in $filesToUpdate) {
    try {
        $content = Get-Content $file.FullName -Raw -ErrorAction SilentlyContinue
        if ($null -ne $content -and ($content.Contains($OldName) -or $content.Contains("L$OldName"))) {

            # Replace L{OldName} => lowercase new name
            $newContent = $content -replace "L$([regex]::Escape($OldName))", $NewProjectName.ToLower()

            # Replace normal OldName => NewProjectName
            $newContent = $newContent -replace [regex]::Escape($OldName), $NewProjectName
            # Use UTF8 encoding to preserve characters, though most C# files are UTF8 with BOM
            Set-Content -Path $file.FullName -Value $newContent -Encoding UTF8
            Write-Host "  Updated: $($file.FullName.Replace($CurrentDir, ''))" -ForegroundColor Green
        }
    } catch {
        Write-Warning "  Could not process $($file.FullName): $($_.Exception.Message)"
    }
}

# 3. Rename files containing "MyProject"
Write-Host "`nRenaming files..." -ForegroundColor Gray
$filesToRename = Get-ChildItem -Path $CurrentDir -Recurse -File | Where-Object {
    $_.Name -match $OldName -and -not ($_.FullName -match "\\\.git\\")
}

foreach ($file in $filesToRename) {
    $newName = $file.Name -replace $OldName, $NewProjectName
    try {
        Rename-Item -Path $file.FullName -NewName $newName -ErrorAction Stop
        Write-Host "  Renamed File: $($file.Name) -> $newName" -ForegroundColor Yellow
    } catch {
        Write-Warning "  Could not rename $($file.Name): $($_.Exception.Message)"
    }
}

# 4. Rename directories containing "MyProject" (Bottom-Up)
Write-Host "`nRenaming directories..." -ForegroundColor Gray
$dirsToRename = Get-ChildItem -Path $CurrentDir -Recurse -Directory | Where-Object {
    $_.Name -match $OldName -and -not ($_.FullName -match "\\\.git\\")
} | Sort-Object -Property @{Expression={$_.FullName.Length}; Descending=$true}

foreach ($dir in $dirsToRename) {
    $newName = $dir.Name -replace $OldName, $NewProjectName
    try {
        Rename-Item -Path $dir.FullName -NewName $newName -ErrorAction Stop
        Write-Host "  Renamed Dir:  $($dir.Name) -> $newName" -ForegroundColor Yellow
    } catch {
        Write-Warning "  Could not rename $($dir.Name): $($_.Exception.Message)"
    }
}

Write-Host "`nProject renaming completed successfully!" -ForegroundColor Green
Write-Host "New project name: $NewProjectName" -ForegroundColor White
Write-Host "Note: You may need to restart your IDE (e.g., Visual Studio) to reflect the changes." -ForegroundColor Gray