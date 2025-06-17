@echo off
title RedBracket CADMATE 2025 Plugin Installer
echo =============================================
echo  RedBracket CADMATE 2025 Plugin Installation
echo =============================================

:: Check if running as administrator
net session >nul 2>&1
if %errorLevel% == 0 (
    echo Running with administrator privileges
) else (
    echo Please run this script as Administrator
    echo Right-click and select "Run as administrator"
    pause
    exit /b 1
)

:: Set paths
set "SOURCE_DIR=%~dp0"
set "CADMATE_DIR=%PROGRAMFILES%\CADMATE\CADMATE 2025"
set "PLUGIN_DIR=%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns"
set "REDBRACKET_PLUGIN_DIR=%PLUGIN_DIR%\RedBracket"

:: Check if CADMATE is installed
if not exist "%CADMATE_DIR%\acad.exe" (
    echo CADMATE 2025 is not installed in the default location.
    echo Please install CADMATE 2025 first.
    pause
    exit /b 1
)

echo Creating plugin directory...
mkdir "%REDBRACKET_PLUGIN_DIR%" 2>nul

:: Copy files
echo Copying files...
copy /Y "%SOURCE_DIR%\AutocadPlugIn\bin\x64\Release\*.dll" "%REDBRACKET_PLUGIN_DIR%\"
copy /Y "%SOURCE_DIR%\AutocadPlugIn\bin\x64\Release\*.pdb" "%REDBRACKET_PLUGIN_DIR%\" 2>nul
copy /Y "%SOURCE_DIR%\grxsdk\inc\*.dll" "%REDBRACKET_PLUGIN_DIR%\"

:: Create plugin loader file
echo Creating plugin loader...
echo ^<?xml version="1.0" encoding="utf-8" ^?> > "%PLUGIN_DIR%\RedBracket.plugin"
echo ^<Plugin name="RedBracket"^> >> "%PLUGIN_DIR%\RedBracket.plugin"
echo   ^<Runtime^> >> "%PLUGIN_DIR%\RedBracket.plugin"
echo     ^<Command^>NETLOAD^</Command^> >> "%PLUGIN_DIR%\RedBracket.plugin"
echo     ^<File^>%REDBRACKET_PLUGIN_DIR%\RBAutocadPlugIn.dll^</File^> >> "%PLUGIN_DIR%\RedBracket.plugin"
echo   ^</Runtime^> >> "%PLUGIN_DIR%\RedBracket.plugin"
echo   ^<Description^>RedBracket CADMATE Integration^</Description^> >> "%PLUGIN_DIR%\RedBracket.plugin"
echo   ^<Company^>RedBracket^</Company^> >> "%PLUGIN_DIR%\RedBracket.plugin"
echo   ^<Version^>1.0.0^</Version^> >> "%PLUGIN_DIR%\RedBracket.plugin"
echo   ^<IsAutoLoad^>True^</IsAutoLoad^> >> "%PLUGIN_DIR%\RedBracket.plugin"
echo   ^<IsLoadOnStartup^>True^</IsLoadOnStartup^> >> "%PLUGIN_DIR%\RedBracket.plugin"
echo ^</Plugin^> >> "%PLUGIN_DIR%\RedBracket.plugin"

echo.
echo =============================================
echo  Installation Complete!
echo =============================================
echo.
echo The RedBracket plugin has been installed to:
echo %REDBRACKET_PLUGIN_DIR%
echo.
echo To complete the setup:
echo 1. Start CADMATE 2025
echo 2. The plugin should load automatically

echo.
echo If you encounter any issues, please refer to the README.txt file.
echo.
pause
