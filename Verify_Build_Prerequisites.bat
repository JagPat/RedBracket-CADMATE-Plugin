@echo off
title RedBracket CADMATE 2025 - Build Prerequisites Check
echo =============================================
echo  Build Prerequisites Verification
echo =============================================

echo Checking system requirements...
echo.

:: Check Windows version
echo [1/6] Checking Windows version...
ver | findstr /i "10\." >nul
if %errorLevel% == 0 (
    echo ✓ Windows 10/11 detected
) else (
    ver | findstr /i "11\." >nul
    if %errorLevel% == 0 (
        echo ✓ Windows 11 detected
    ) else (
        echo ✗ Windows 10/11 required
    )
)

:: Check for 64-bit system
echo [2/6] Checking system architecture...
if "%PROCESSOR_ARCHITECTURE%"=="AMD64" (
    echo ✓ 64-bit system detected
) else (
    echo ✗ 64-bit system required
)

:: Check for Visual Studio
echo [3/6] Checking for Visual Studio...
if exist "%ProgramFiles%\Microsoft Visual Studio\2022" (
    echo ✓ Visual Studio 2022 found
) else if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019" (
    echo ✓ Visual Studio 2019 found
) else if exist "%ProgramFiles%\Microsoft Visual Studio\2019" (
    echo ✓ Visual Studio 2019 found
) else (
    echo ✗ Visual Studio 2019+ not found
    echo   Download from: https://visualstudio.microsoft.com/downloads/
)

:: Check for .NET Framework 4.7.2
echo [4/6] Checking .NET Framework 4.7.2...
reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" /v Release 2>nul | findstr "461808" >nul
if %errorLevel% == 0 (
    echo ✓ .NET Framework 4.7.2+ installed
) else (
    echo ✗ .NET Framework 4.7.2 required
    echo   Download from: https://dotnet.microsoft.com/download/dotnet-framework/net472
)

:: Check for CADMATE 2025
echo [5/6] Checking for CADMATE 2025...
if exist "%ProgramFiles%\CADMATE\CADMATE 2025" (
    echo ✓ CADMATE 2025 installation found
) else (
    echo ⚠ CADMATE 2025 not found in default location
    echo   Required for testing the plugin
)

:: Check for MSBuild
echo [6/6] Checking for MSBuild...
where msbuild >nul 2>&1
if %errorLevel% == 0 (
    echo ✓ MSBuild found in PATH
) else (
    echo ⚠ MSBuild not in PATH - Use Developer Command Prompt
)

echo.
echo =============================================
echo  Prerequisites Check Complete
echo =============================================
echo.
echo If any items show ✗, please install them before building.
echo If items show ⚠, they may cause issues but aren't critical for building.
echo.
pause
