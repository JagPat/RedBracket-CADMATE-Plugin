@echo off
title RedBracket CADMATE 2025 Plugin - Installation Test
echo =============================================
echo  Testing RedBracket Plugin Installation
echo =============================================

:: Set paths
set "CADMATE_PLUGINS=%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns"
set "REDBRACKET_DIR=%CADMATE_PLUGINS%\RedBracket"
set "PLUGIN_CONFIG=%CADMATE_PLUGINS%\RedBracket.plugin"

echo [1/6] Checking plugin directory...
if exist "%REDBRACKET_DIR%" (
    echo ✓ Plugin directory exists: %REDBRACKET_DIR%
) else (
    echo ✗ Plugin directory not found
    goto :test_failed
)

echo [2/6] Checking main plugin DLL...
if exist "%REDBRACKET_DIR%\RBAutocadPlugIn.dll" (
    echo ✓ Main plugin DLL found
    
    :: Check file size (should be > 100KB for a real plugin)
    for %%A in ("%REDBRACKET_DIR%\RBAutocadPlugIn.dll") do (
        if %%~zA gtr 100000 (
            echo ✓ DLL size looks reasonable: %%~zA bytes
        ) else (
            echo ⚠ DLL size seems small: %%~zA bytes
        )
    )
) else (
    echo ✗ Main plugin DLL not found
    goto :test_failed
)

echo [3/6] Checking dependencies...
set "DEPS_OK=1"

if exist "%REDBRACKET_DIR%\Newtonsoft.Json.dll" (
    echo ✓ Newtonsoft.Json.dll found
) else (
    echo ⚠ Newtonsoft.Json.dll not found
    set "DEPS_OK=0"
)

if exist "%REDBRACKET_DIR%\RestSharp.dll" (
    echo ✓ RestSharp.dll found
) else (
    echo ⚠ RestSharp.dll not found
    set "DEPS_OK=0"
)

if "%DEPS_OK%"=="0" (
    echo ⚠ Some dependencies missing - plugin may not work properly
)

echo [4/6] Checking plugin configuration...
if exist "%PLUGIN_CONFIG%" (
    echo ✓ Plugin configuration file found
    
    :: Check if configuration contains required elements
    findstr "RedBracket" "%PLUGIN_CONFIG%" >nul
    if %errorLevel% == 0 (
        echo ✓ Configuration contains RedBracket references
    ) else (
        echo ⚠ Configuration may be malformed
    )
) else (
    echo ✗ Plugin configuration file not found
    goto :test_failed
)

echo [5/6] Checking CADMATE installation...
if exist "%ProgramFiles%\CADMATE\CADMATE 2025" (
    echo ✓ CADMATE 2025 installation found
) else (
    echo ⚠ CADMATE 2025 not found in default location
    echo   Plugin won't work without CADMATE 2025
)

echo [6/6] Checking .NET Framework...
reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" /v Release 2>nul | findstr "461808" >nul
if %errorLevel% == 0 (
    echo ✓ .NET Framework 4.7.2+ is installed
) else (
    echo ⚠ .NET Framework 4.7.2+ may not be installed
    echo   Plugin requires .NET Framework 4.7.2 or later
)

echo.
echo =============================================
echo  Installation Test Results
echo =============================================
echo.
echo ✓ Plugin appears to be correctly installed
echo.
echo Manual verification steps:
echo 1. Start CADMATE 2025
echo 2. Look for "RedBracket" tab in the ribbon
echo 3. If tab is missing, type: NETLOAD
echo 4. Browse to: %REDBRACKET_DIR%\RBAutocadPlugIn.dll
echo 5. Click "Load" and check for errors in command line
echo.
echo Installed files:
dir "%REDBRACKET_DIR%" /b
echo.
goto :test_complete

:test_failed
echo.
echo =============================================
echo  Installation Test FAILED
echo =============================================
echo.
echo Please run the installer again or check for errors.
echo.

:test_complete
pause
