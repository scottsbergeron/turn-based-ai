param(
    [Parameter(Mandatory=$false)]
    [ValidateSet('windows', 'mac', 'linux', 'all')]
    [string]$Platform = 'windows'
)

Write-Host "Building TurnBasedGame for platform: $Platform"
Write-Host "Note: Docker must be installed and running."

# Create and prepare publish directories
$publishDirs = @("windows", "mac", "linux")
foreach ($dir in $publishDirs) {
    $path = "publish/$dir"
    
    # Create directory if it doesn't exist
    if (-not (Test-Path $path)) {
        New-Item -ItemType Directory -Force -Path $path | Out-Null
    }
    
    # Clean the directory
    Get-ChildItem -Path $path -Recurse | Remove-Item -Force -Recurse
    
    # Ensure directory has write permissions
    $acl = Get-Acl $path
    $identity = [System.Security.Principal.WindowsIdentity]::GetCurrent().Name
    $fileSystemRights = [System.Security.AccessControl.FileSystemRights]::FullControl
    $type = [System.Security.AccessControl.AccessControlType]::Allow
    $inheritanceFlags = [System.Security.AccessControl.InheritanceFlags]::ContainerInherit -bor [System.Security.AccessControl.InheritanceFlags]::ObjectInherit
    $propagationFlags = [System.Security.AccessControl.PropagationFlags]::None
    $accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($identity, $fileSystemRights, $inheritanceFlags, $propagationFlags, $type)
    $acl.SetAccessRule($accessRule)
    Set-Acl -Path $path -AclObject $acl
}

if ($Platform -eq 'all') {
    Write-Host "`nBuilding for all platforms..."
    docker-compose build build-windows build-mac build-linux
    docker-compose up --force-recreate build-windows build-mac build-linux
} else {
    Write-Host "`nBuilding for $Platform..."
    docker-compose build "build-$Platform"
    docker-compose up --force-recreate "build-$Platform"
}

# Verify output files exist
$targetDir = "publish/$Platform"
if (!(Test-Path "$targetDir/TurnBasedGame.exe") -and !(Test-Path "$targetDir/TurnBasedGame")) {
    Write-Host "Error: Build files not found in output directory!" -ForegroundColor Red
    exit 1
}

Write-Host "`nBuild complete!"
Write-Host "Output locations:"
Write-Host "  Windows: .\publish\windows\TurnBasedGame.exe"
Write-Host "  Mac: ./publish/mac/TurnBasedGame"
Write-Host "  Linux: ./publish/linux/TurnBasedGame"
Write-Host "`nTo run the game:"
Write-Host "  Windows: .\publish\windows\TurnBasedGame.exe"
Write-Host "  Mac: chmod +x ./publish/mac/TurnBasedGame && ./publish/mac/TurnBasedGame"
Write-Host "  Linux: chmod +x ./publish/linux/TurnBasedGame && ./publish/linux/TurnBasedGame"
