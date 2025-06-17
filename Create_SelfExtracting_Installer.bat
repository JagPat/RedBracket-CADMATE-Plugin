@echo off
title Create RedBracket CADMATE 2025 Self-Extracting Installer
echo =========================================================
echo  Creating Self-Extracting Installer for RedBracket Plugin
echo =========================================================

:: Set paths
set "SOURCE_DIR=%~dp0"
set "OUTPUT_FILE=RedBracket_CADMATE2025_Plugin_Installer.exe"
set "TEMP_DIR=%TEMP%\RedBracket_Installer"

:: Create temporary directory
echo Preparing files...
if exist "%TEMP_DIR%" rmdir /s /q "%TEMP_DIR%"
mkdir "%TEMP_DIR%"

:: Copy required files
echo Copying plugin files...

:: Main plugin files
xcopy /Y /E /I "%SOURCE_DIR%AutocadPlugIn\bin\x64\Release\*.dll" "%TEMP_DIR%\"
xcopy /Y /E /I "%SOURCE_DIR%AutocadPlugIn\bin\x64\Release\*.pdb" "%TEMP_DIR%\"

:: Copy GRX SDK files
xcopy /Y /E /I "%SOURCE_DIR%grxsdk\inc\*.dll" "%TEMP_DIR%\"

:: Copy installer script
copy /Y "%SOURCE_DIR%RedBracket_Plugin_Installer.bat" "%TEMP_DIR%\"
copy /Y "%SOURCE_DIR%README.txt" "%TEMP_DIR%\"

:: Create SFX config file
echo Creating installer configuration...
echo ;@echo off > "%TEMP_DIR%\config.txt"
echo Path=%%TEMP%%\RedBracket_Extracted >> "%TEMP_DIR%\config.txt"
echo SavePath=>> "%TEMP_DIR%\config.txt"
echo Title=RedBracket CADMATE 2025 Plugin Installer>> "%TEMP_DIR%\config.txt"
echo BeginPrompt=This will install RedBracket Plugin for CADMATE 2025. Continue?>> "%TEMP_DIR%\config.txt"
echo RunProgram="RedBracket_Plugin_Installer.bat">> "%TEMP_DIR%\config.txt"

:: Create SFX using PowerShell
echo Creating self-extracting archive...
powershell -command "
    $source = '%TEMP_DIR%\*';
    $destination = '%SOURCE_DIR%%OUTPUT_FILE%';
    $config = '%TEMP_DIR%\config.txt';
    
    # Create a ZIP file
    Compress-Archive -Path $source -DestinationPath "$env:TEMP\temp.zip" -Force;
    
    # Create SFX header
    $sfx = [System.IO.File]::ReadAllBytes('C:\Windows\System32\extrac32.exe');
    $zip = [System.IO.File]::ReadAllBytes("$env:TEMP\temp.zip");
    $configBytes = [System.Text.Encoding]::ASCII.GetBytes((Get-Content $config -Raw) + "`r`n");
    
    # Combine SFX header, config, and ZIP
    $combined = New-Object byte[]($sfx.Length + $configBytes.Length + $zip.Length);
    [System.Buffer]::BlockCopy($sfx, 0, $combined, 0, $sfx.Length);
    [System.Buffer]::BlockCopy($configBytes, 0, $combined, $sfx.Length, $configBytes.Length);
    [System.Buffer]::BlockCopy($zip, 0, $combined, $sfx.Length + $configBytes.Length, $zip.Length);
    
    # Save as SFX
    [System.IO.File]::WriteAllBytes($destination, $combined);
    
    # Clean up
    Remove-Item "$env:TEMP\temp.zip" -Force;
"

:: Cleanup
rmdir /s /q "%TEMP_DIR%"

echo.
echo =============================================
echo  Self-extracting installer created successfully!
echo  File: %OUTPUT_FILE%
echo =============================================
echo.
pause
