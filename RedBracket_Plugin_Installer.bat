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
    echo This installer requires administrator privileges.
    echo Please right-click and select "Run as administrator"
    pause
    exit /b 1
)

:: Set paths
set "INSTALL_DIR=%PROGRAMFILES%\RedBracket\CADMATE2025_Plugin"
set "APPDATA_DIR=%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket"
set "PLUGIN_FILE=%APPDATA_DIR%\RBAutocadPlugIn.dll"

:: Create installation directory
echo Creating installation directory...
mkdir "%INSTALL_DIR%" 2>nul
if not exist "%INSTALL_DIR%" (
    echo Failed to create installation directory
    pause
    exit /b 1
)

:: Extract files (this will be handled by the SFX)
:: The following files will be included in the SFX:
:: - RBAutocadPlugIn.dll
:: - All required GRX SDK DLLs
:: - README.txt

:: Copy files to AppData
echo Installing plugin files...
mkdir "%APPDATA_DIR%" 2>nul
xcopy /Y /E /I "%INSTALL_DIR%\*" "%APPDATA_DIR%\"

:: Create plugin loader
echo Configuring CADMATE...
echo ^<?xml version="1.0" encoding="utf-8" ^?> > "%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket.plugin"
echo ^<Plugin name="RedBracket"^> >> "%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket.plugin"
echo   ^<Runtime^> >> "%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket.plugin"
echo     ^<Command^>NETLOAD^</Command^> >> "%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket.plugin"
echo     ^<File^>%PLUGIN_FILE%^</File^> >> "%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket.plugin"
echo   ^</Runtime^> >> "%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket.plugin"
echo   ^<Description^>RedBracket CADMATE Integration^</Description^> >> "%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket.plugin"
echo   ^<Company^>RedBracket^</Company^> >> "%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket.plugin"
echo   ^<Version^>1.0.0^</Version^> >> "%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket.plugin"
echo   ^<IsAutoLoad^>True^</IsAutoLoad^> >> "%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket.plugin"
echo   ^<IsLoadOnStartup^>True^</IsLoadOnStartup^> >> "%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket.plugin"
echo ^</Plugin^> >> "%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket.plugin"

:: Create desktop shortcut
echo Creating desktop shortcut...
set "SHORTCUT=%USERPROFILE%\Desktop\RedBracket CADMATE Plugin.lnk"
set "TARGET=%SystemRoot%\System32\cmd.exe"
set "ICON=%APPDATA_DIR%\RBAutocadPlugIn.dll,0"

powershell -command "$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%SHORTCUT%'); $Shortcut.TargetPath = '%TARGET%'; $Shortcut.Arguments = '/k echo RedBracket CADMATE 2025 Plugin is installed. Start CADMATE 2025 to use it. && pause'; $Shortcut.IconLocation = '%ICON%'; $Shortcut.Description = 'RedBracket CADMATE 2025 Plugin'; $Shortcut.Save()"

echo.
echo =============================================
echo  Installation Complete!
echo =============================================
echo.
echo The RedBracket plugin has been installed successfully.
echo.
echo Next steps:
echo 1. Start CADMATE 2025
echo 2. The RedBracket tab should appear in the ribbon
echo 3. If not, type RIBBON in the command line and enable the RedBracket tab
echo.
echo A shortcut has been created on your desktop with more information.
echo.
pause
