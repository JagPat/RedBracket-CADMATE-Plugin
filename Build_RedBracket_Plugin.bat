@echo off
title RedBracket CADMATE 2025 - Build Script
echo =============================================
echo  Building RedBracket CADMATE 2025 Plugin
echo =============================================

:: Set paths
set "SOURCE_DIR=%~dp0"
set "SOLUTION_FILE=%SOURCE_DIR%CADPlugIns.sln"
set "PROJECT_FILE=%SOURCE_DIR%AutocadPlugIn\RBAutocadPlugIn.csproj"
set "OUTPUT_DIR=%SOURCE_DIR%AutocadPlugIn\bin\x64\Release"

:: Check if solution file exists
if not exist "%SOLUTION_FILE%" (
    echo ✗ Solution file not found: CADPlugIns.sln
    echo   Make sure you're running this from the project root directory
    pause
    exit /b 1
)

echo ✓ Found solution file: CADPlugIns.sln
echo.

:: Check for MSBuild
echo [1/4] Locating MSBuild...
where msbuild >nul 2>&1
if %errorLevel% == 0 (
    set "MSBUILD_PATH=msbuild"
    echo ✓ MSBuild found in PATH
) else (
    :: Try to find MSBuild in VS 2022
    if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe" (
        set "MSBUILD_PATH=%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
    ) else if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" (
        set "MSBUILD_PATH=%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
    ) else if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" (
        set "MSBUILD_PATH=%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
    ) else if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe" (
        set "MSBUILD_PATH=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
    ) else if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" (
        set "MSBUILD_PATH=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
    ) else if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" (
        set "MSBUILD_PATH=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
    ) else (
        echo ✗ MSBuild not found
        echo   Please open "Developer Command Prompt for VS" and run this script from there
        pause
        exit /b 1
    )
    echo ✓ Found MSBuild: %MSBUILD_PATH%
)

:: Clean previous builds
echo [2/4] Cleaning previous builds...
"%MSBUILD_PATH%" "%SOLUTION_FILE%" /t:Clean /p:Configuration=Release /p:Platform=x64 /v:minimal
if %errorLevel% neq 0 (
    echo ⚠ Clean failed, continuing anyway...
)

:: Restore NuGet packages
echo [3/4] Restoring packages...
"%MSBUILD_PATH%" "%SOLUTION_FILE%" /t:Restore /p:Configuration=Release /p:Platform=x64 /v:minimal
if %errorLevel% neq 0 (
    echo ✗ Package restore failed
    pause
    exit /b 1
)

:: Build the solution
echo [4/4] Building solution...
"%MSBUILD_PATH%" "%SOLUTION_FILE%" /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal
if %errorLevel% neq 0 (
    echo ✗ Build failed
    echo.
    echo Common solutions:
    echo 1. Open Visual Studio and build manually to see detailed errors
    echo 2. Check that all GRX SDK references are correctly pointing to your CADMATE installation
    echo 3. Ensure .NET Framework 4.7.2 Developer Pack is installed
    pause
    exit /b 1
)

:: Verify output files
echo.
echo =============================================
echo  Build Verification
echo =============================================
echo.

if exist "%OUTPUT_DIR%\RBAutocadPlugIn.dll" (
    echo ✓ Main plugin DLL: RBAutocadPlugIn.dll
) else (
    echo ✗ Main plugin DLL not found
    goto :build_failed
)

if exist "%OUTPUT_DIR%\Newtonsoft.Json.dll" (
    echo ✓ Dependency: Newtonsoft.Json.dll
) else (
    echo ⚠ Newtonsoft.Json.dll not found
)

if exist "%OUTPUT_DIR%\RestSharp.dll" (
    echo ✓ Dependency: RestSharp.dll
) else (
    echo ⚠ RestSharp.dll not found
)

:: List all output files
echo.
echo Built files in %OUTPUT_DIR%:
dir "%OUTPUT_DIR%\*.dll" /b 2>nul
dir "%OUTPUT_DIR%\*.pdb" /b 2>nul

echo.
echo =============================================
echo  Build Successful!
echo =============================================
echo.
echo Next steps:
echo 1. Run "Create_Installation_Package.bat" to create installer
echo 2. Copy the installer to target machines
echo 3. Test the plugin in CADMATE 2025
echo.
pause
exit /b 0

:build_failed
echo.
echo =============================================
echo  Build Failed!
echo =============================================
echo.
echo Please check the error messages above and try building in Visual Studio
echo for more detailed error information.
echo.
pause
exit /b 1
