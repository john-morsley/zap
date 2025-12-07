# Run-ZAP-Headless.ps1
# Runs OWASP ZAP in headless mode against each split Swagger file

param(
    [string]$SwaggerFolder = "..\swagger",
    [string]$ZapPath = "C:\Program Files\ZAP\Zed Attack Proxy\zap.bat",
    [string]$OutputFolder = "..\reports",
    [string]$TargetUrl = "http://localhost:5000",
    [string]$PlanFile = "..\plans\simple-zap-automation-plan.yaml"
)

# Ensure output folder exists
if (-not (Test-Path $OutputFolder)) {
    New-Item -ItemType Directory -Path $OutputFolder -Force | Out-Null
    Write-Host "Created output folder: $OutputFolder" -ForegroundColor Green
}

# Check if ZAP exists
if (-not (Test-Path $ZapPath)) {
    Write-Host "ERROR: ZAP not found at: $ZapPath" -ForegroundColor Red
    exit 1
}

# Check if Swagger folder exists
if (-not (Test-Path $SwaggerFolder)) {
    Write-Host "ERROR: Swagger folder not found at: $SwaggerFolder" -ForegroundColor Red
    exit 1
}

# Get all Swagger JSON files
$swaggerFiles = Get-ChildItem -Path $SwaggerFolder -Filter "*.json"

if ($swaggerFiles.Count -eq 0) {
    Write-Host "ERROR: No Swagger JSON files found in: $SwaggerFolder" -ForegroundColor Red
    exit 1
}

Write-Host "`nFound $($swaggerFiles.Count) Swagger files to scan" -ForegroundColor Cyan
Write-Host "============================================`n" -ForegroundColor DarkGray

foreach ($file in $swaggerFiles) {
    $fileName = $file.BaseName
    $fullPath = $file.FullName
    $reportName = "$fileName-report"
    $reportPath = Join-Path $OutputFolder $reportName
    
    Write-Host "Processing: $fileName" -ForegroundColor Yellow
    Write-Host "  Swagger file: $fullPath" -ForegroundColor DarkGray
    Write-Host "  Report will be saved to: $reportPath" -ForegroundColor DarkGray
    
    # Create a temporary plan file with the specific swagger file path
    $tempPlanFile = Join-Path $env:TEMP "$fileName-plan.yaml"
    $planContent = Get-Content $PlanFile -Raw
    
    # Get absolute paths for scripts (convert backslashes to forward slashes for YAML)
    $scriptsDir = (Resolve-Path "..\scripts").Path -replace '\\', '/'
    $authScript = "$scriptsDir/auth.js"
    $sessionScript = "$scriptsDir/session.js"
    $httpsenderScript = "$scriptsDir/httpsender.js"
    
    # Replace script paths with absolute paths
    $planContent = $planContent -replace 'script:\s*"\.\.\/scripts\/auth\.js"', "script: `"$authScript`""
    $planContent = $planContent -replace 'script:\s*"\.\.\/scripts\/session\.js"', "script: `"$sessionScript`""
    $planContent = $planContent -replace 'file:\s*"\.\.\/scripts\/httpsender\.js"', "file: `"$httpsenderScript`""
    
    # Replace the apiFile path with the current swagger file (use absolute path)
    $swaggerAbsolutePath = $fullPath -replace '\\', '/'
    $planContent = $planContent -replace 'apiFile:\s*"[^"]*swagger[^"]*"', "apiFile: `"$swaggerAbsolutePath`""
    
    # Replace the reportDir with absolute path
    $absoluteReportDir = (Resolve-Path $OutputFolder).Path -replace '\\', '/'
    $planContent = $planContent -replace 'reportDir:\s*"[^"]*"', "reportDir: `"$absoluteReportDir`""
    
    # Replace the reportFile to include the swagger file name
    $planContent = $planContent -replace 'reportFile:\s*"[^"]*"', "reportFile: `"$reportName-{{yyyy-MM-dd-HH-mm-ss}}`""
    
    # Write the modified plan to temp file
    $planContent | Set-Content $tempPlanFile -Encoding UTF8
    
    $zapArgs = @(
        "-cmd",
        "-autorun", $tempPlanFile
    )
    
    Write-Host "  Starting ZAP scan..." -ForegroundColor Green
    
    try {
        # Change to ZAP directory to run the command
        $zapDir = Split-Path $ZapPath -Parent
        $zapExe = Split-Path $ZapPath -Leaf
        
        Push-Location $zapDir
        & ".\$zapExe" $zapArgs
        Pop-Location
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  Scan completed successfully" -ForegroundColor Green
        } else {
            Write-Host "  Scan completed with warnings (exit code: $LASTEXITCODE)" -ForegroundColor Yellow
        }
    }
    catch {
        Pop-Location
        Write-Host "  ERROR: $($_.Exception.Message)" -ForegroundColor Red
    }
    finally {
        # Clean up temp plan file
        if (Test-Path $tempPlanFile) {
            Remove-Item $tempPlanFile -Force
        }
    }
    
    Write-Host ""
}

Write-Host "============================================" -ForegroundColor DarkGray
Write-Host "All scans completed!" -ForegroundColor Green
Write-Host "Reports saved to: $OutputFolder" -ForegroundColor Cyan
