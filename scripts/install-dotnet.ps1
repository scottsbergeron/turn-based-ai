Write-Host "Starting .NET SDK installation check..."
Write-Host "Current directory: $(Get-Location)"
Write-Host "Running as administrator: $([bool](([System.Security.Principal.WindowsIdentity]::GetCurrent()).groups -match 'S-1-5-32-544'))"

# Check if .NET SDK 8.0 is installed (more robust check)
$dotnetInstalled = $false
try {
    $dotnetVersion = dotnet --version
    if ($dotnetVersion -match "8.0") {
        $dotnetInstalled = $true
    }
} catch {
    $dotnetInstalled = $false
}

if ($dotnetInstalled) {
    Write-Host ".NET SDK 8.0 is already installed."
} else {
    Write-Host ".NET SDK 8.0 is not installed. Installing..."
    
    # Download the .NET SDK installer
    $installerUrl = "https://download.visualstudio.microsoft.com/download/pr/2b2d6133-c4f9-46dd-9ab6-86443a7f5783/340054e2ac7de2bff9eea73ec9d4995a/dotnet-sdk-8.0.100-win-x64.exe"
    $installerPath = "$env:TEMP\dotnet-sdk-8.0.100-win-x64.exe"
    
    try {
        # Configure TLS and WebClient
        [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
        $webClient = New-Object System.Net.WebClient
        
        # Download the installer
        Write-Host "Downloading .NET SDK installer..."
        Write-Host "From: $installerUrl"
        Write-Host "To: $installerPath"
        
        try {
            $webClient.DownloadFile($installerUrl, $installerPath)
        } catch {
            Write-Host "Download failed with WebClient. Trying alternative method..."
            Start-BitsTransfer -Source $installerUrl -Destination $installerPath
        }
        
        if (Test-Path $installerPath) {
            # Run the installer
            Write-Host "Installing .NET SDK..."
            $process = Start-Process -FilePath $installerPath -ArgumentList "/quiet" -Wait -PassThru
            Write-Host "Installer exit code: $($process.ExitCode)"
            
            # Clean up
            Remove-Item $installerPath
            
            # Verify installation
            try {
                $env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")
                $newVersion = dotnet --version
                if ($newVersion -match "8.0") {
                    Write-Host ".NET SDK 8.0 has been successfully installed."
                } else {
                    Write-Host "Installation may have failed. Please install .NET SDK 8.0 manually."
                    Write-Host "Visit: https://dotnet.microsoft.com/download/dotnet/8.0"
                }
            } catch {
                Write-Host "Installation verification failed: $_"
                Write-Host "Please install .NET SDK 8.0 manually."
                Write-Host "Visit: https://dotnet.microsoft.com/download/dotnet/8.0"
            }
        } else {
            Write-Host "Failed to download installer. File not found at: $installerPath"
            Write-Host "Please download and install .NET SDK 8.0 manually from: https://dotnet.microsoft.com/download/dotnet/8.0"
        }
    }
    catch {
        Write-Host "An error occurred during installation: $_"
        Write-Host "Error details:"
        Write-Host $_.Exception.Message
        Write-Host "Please install .NET SDK 8.0 manually from: https://dotnet.microsoft.com/download/dotnet/8.0"
    }
}

# Check for MonoGame dependencies
Write-Host "`nChecking MonoGame dependencies..."

# Check if OpenAL is installed (this is a basic check, might need refinement)
$openALPath = "C:\Windows\System32\OpenAL32.dll"
if (Test-Path $openALPath) {
    Write-Host "OpenAL is installed."
} else {
    Write-Host "OpenAL might not be installed. You may need to install it manually if you encounter audio issues."
    Write-Host "Visit: https://www.openal.org/downloads/"
}

Write-Host "`nSetup complete! You can now build and run the game."
Write-Host "To build: docker-compose up --build"
Write-Host "To run: cd publish && dotnet TurnBasedGame.dll"
