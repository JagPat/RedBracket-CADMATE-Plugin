@echo off
title RedBracket CADMATE 2025 - Prepare for Windows Build
echo =============================================
echo  Preparing Source Code for Windows Build
echo =============================================

:: Check if we're running on Windows
ver | findstr /i "Windows" >nul
if %errorLevel% neq 0 (
    echo This script must be run on Windows for building.
    echo.
    echo If you're currently on Linux/Mac:
    echo 1. Copy all project files to a Windows machine
    echo 2. Install Visual Studio 2019+ with .NET desktop development
    echo 3. Install CADMATE 2025
    echo 4. Run this script on the Windows machine
    echo.
    echo Files needed on Windows machine:
    echo ✓ All .cs source files
    echo ✓ .csproj project files  
    echo ✓ .sln solution file
    echo ✓ grxsdk folder
    echo ✓ Binaries folder
    echo ✓ All .bat deployment scripts
    echo.
    pause
    exit /b 1
)

echo ✓ Running on Windows - ready for build process
echo.

:: Set paths
set "PROJECT_ROOT=%~dp0"
set "CHECKLIST_FILE=%PROJECT_ROOT%Windows_Build_Checklist.txt"

echo Creating build checklist...
echo RedBracket CADMATE 2025 Plugin - Windows Build Checklist > "%CHECKLIST_FILE%"
echo ============================================================= >> "%CHECKLIST_FILE%"
echo. >> "%CHECKLIST_FILE%"
echo Prerequisites Checklist: >> "%CHECKLIST_FILE%"
echo [ ] Windows 10/11 (64-bit) >> "%CHECKLIST_FILE%"
echo [ ] Visual Studio 2019 or later >> "%CHECKLIST_FILE%"
echo [ ] .NET Framework 4.7.2 Developer Pack >> "%CHECKLIST_FILE%"
echo [ ] CADMATE 2025 installed >> "%CHECKLIST_FILE%"
echo [ ] All source files copied to Windows machine >> "%CHECKLIST_FILE%"
echo. >> "%CHECKLIST_FILE%"
echo Build Process: >> "%CHECKLIST_FILE%"
echo [ ] 1. Run: Verify_Build_Prerequisites.bat >> "%CHECKLIST_FILE%"
echo [ ] 2. Run: Build_RedBracket_Plugin.bat >> "%CHECKLIST_FILE%"
echo [ ] 3. Run: Create_Installation_Package.bat >> "%CHECKLIST_FILE%"
echo [ ] 4. Test: Test_Plugin_Installation.bat >> "%CHECKLIST_FILE%"
echo. >> "%CHECKLIST_FILE%"
echo Deployment: >> "%CHECKLIST_FILE%"
echo [ ] 5. Copy ZIP package to target machines >> "%CHECKLIST_FILE%"
echo [ ] 6. Run installer as Administrator >> "%CHECKLIST_FILE%"
echo [ ] 7. Test plugin in CADMATE 2025 >> "%CHECKLIST_FILE%"
echo. >> "%CHECKLIST_FILE%"
echo For detailed instructions, see: DEPLOYMENT_GUIDE.md >> "%CHECKLIST_FILE%"

echo ✓ Checklist created: %CHECKLIST_FILE%

:: Check current project structure
echo.
echo Verifying project structure...

if exist "CADPlugIns.sln" (
    echo ✓ Solution file found
) else (
    echo ✗ CADPlugIns.sln not found
)

if exist "AutocadPlugIn\RBAutocadPlugIn.csproj" (
    echo ✓ Main project file found
) else (
    echo ✗ RBAutocadPlugIn.csproj not found
)

if exist "grxsdk\" (
    echo ✓ GRX SDK folder found
) else (
    echo ✗ grxsdk folder not found
)

if exist "Binaries\" (
    echo ✓ Binaries folder found
) else (
    echo ✗ Binaries folder not found
)

echo.
echo =============================================
echo  Ready for Windows Build Process
echo =============================================
echo.
echo Next steps:
echo 1. Review checklist: %CHECKLIST_FILE%
echo 2. Run: Verify_Build_Prerequisites.bat
echo 3. Run: Build_RedBracket_Plugin.bat
echo.
pause
