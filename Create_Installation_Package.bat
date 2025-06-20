@echo off
title RedBracket CADMATE 2025 - Create Installation Package
echo =============================================
echo  Creating Installation Package
echo =============================================

:: Set paths
set "SOURCE_DIR=%~dp0"
set "BUILD_DIR=%SOURCE_DIR%AutocadPlugIn\bin\x64\Release"
set "PACKAGE_DIR=%SOURCE_DIR%RedBracket_CADMATE2025_InstallPackage"
set "ZIP_FILE=%SOURCE_DIR%RedBracket_CADMATE2025_Plugin_v1.5.0.zip"

:: Check if build output exists
if not exist "%BUILD_DIR%\RBAutocadPlugIn.dll" (
    echo ✗ Build output not found. Please run Build_RedBracket_Plugin.bat first
    pause
    exit /b 1
)

:: Clean and create package directory
echo [1/5] Preparing package directory...
if exist "%PACKAGE_DIR%" rmdir /s /q "%PACKAGE_DIR%"
mkdir "%PACKAGE_DIR%"
mkdir "%PACKAGE_DIR%\Plugin"
mkdir "%PACKAGE_DIR%\Documentation"

:: Copy main plugin files
echo [2/5] Copying plugin files...
copy /Y "%BUILD_DIR%\RBAutocadPlugIn.dll" "%PACKAGE_DIR%\Plugin\"
copy /Y "%BUILD_DIR%\RBAutocadPlugIn.pdb" "%PACKAGE_DIR%\Plugin\" 2>nul

:: Copy dependencies
copy /Y "%BUILD_DIR%\Newtonsoft.Json.dll" "%PACKAGE_DIR%\Plugin\" 2>nul
copy /Y "%BUILD_DIR%\RestSharp.dll" "%PACKAGE_DIR%\Plugin\" 2>nul
copy /Y "%BUILD_DIR%\ExpandableGridView.dll" "%PACKAGE_DIR%\Plugin\" 2>nul
copy /Y "%BUILD_DIR%\IOM.dll" "%PACKAGE_DIR%\Plugin\" 2>nul

:: Copy GRX SDK dependencies if they exist
copy /Y "%SOURCE_DIR%grxsdk\inc\*.dll" "%PACKAGE_DIR%\Plugin\" 2>nul

:: Copy configuration files
copy /Y "%BUILD_DIR%\*.config" "%PACKAGE_DIR%\Plugin\" 2>nul
copy /Y "%BUILD_DIR%\acad.lsp" "%PACKAGE_DIR%\Plugin\" 2>nul
copy /Y "%BUILD_DIR%\start.scr" "%PACKAGE_DIR%\Plugin\" 2>nul

:: Copy documentation
echo [3/5] Copying documentation...
copy /Y "%SOURCE_DIR%README.txt" "%PACKAGE_DIR%\Documentation\"
copy /Y "%SOURCE_DIR%BUILD_INSTRUCTIONS.md" "%PACKAGE_DIR%\Documentation\"
copy /Y "%SOURCE_DIR%Binaries\RedBracketConnector_Integration_UserGuide.chm" "%PACKAGE_DIR%\Documentation\" 2>nul

:: Create enhanced installer script
echo [4/5] Creating installer script...
echo @echo off > "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo title RedBracket CADMATE 2025 Plugin - Installation >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo ============================================= >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo  RedBracket CADMATE 2025 Plugin Installation >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo  Version 1.5.0 >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo ============================================= >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo. >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo. >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo :: Check administrator privileges >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo net session ^>nul 2^>^&1 >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo if %%errorLevel%% neq 0 ^( >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo     echo ✗ Administrator privileges required >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo     echo Please right-click and select "Run as administrator" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo     pause >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo     exit /b 1 >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo ^) >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo. >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo :: Set installation paths >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo set "INSTALL_DIR=%%~dp0" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo set "CADMATE_PLUGINS=%%APPDATA%%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo set "REDBRACKET_DIR=%%CADMATE_PLUGINS%%\RedBracket" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo. >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo :: Create plugin directory >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo [1/3] Creating plugin directory... >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo mkdir "%%REDBRACKET_DIR%%" 2^>nul >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo. >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo :: Copy plugin files >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo [2/3] Installing plugin files... >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo xcopy /Y /E /I "%%INSTALL_DIR%%Plugin\*" "%%REDBRACKET_DIR%%\" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo if %%errorLevel%% neq 0 ^( >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo     echo ✗ File copy failed >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo     pause >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo     exit /b 1 >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo ^) >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo. >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo :: Create plugin loader configuration >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo [3/3] Configuring plugin loader... >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo ^(echo ^<?xml version="1.0" encoding="utf-8" ?^>^) ^> "%%CADMATE_PLUGINS%%\RedBracket.plugin" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo ^(echo ^<Plugin name="RedBracket"^>^) ^>^> "%%CADMATE_PLUGINS%%\RedBracket.plugin" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo ^(echo   ^<Runtime^>^) ^>^> "%%CADMATE_PLUGINS%%\RedBracket.plugin" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo ^(echo     ^<Command^>NETLOAD^</Command^>^) ^>^> "%%CADMATE_PLUGINS%%\RedBracket.plugin" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo ^(echo     ^<File^>%%REDBRACKET_DIR%%\RBAutocadPlugIn.dll^</File^>^) ^>^> "%%CADMATE_PLUGINS%%\RedBracket.plugin" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo ^(echo   ^</Runtime^>^) ^>^> "%%CADMATE_PLUGINS%%\RedBracket.plugin" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo ^(echo   ^<Description^>RedBracket CADMATE Integration v1.5.0^</Description^>^) ^>^> "%%CADMATE_PLUGINS%%\RedBracket.plugin" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo ^(echo   ^<Company^>RedBracket^</Company^>^) ^>^> "%%CADMATE_PLUGINS%%\RedBracket.plugin" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo ^(echo   ^<Version^>1.5.0^</Version^>^) ^>^> "%%CADMATE_PLUGINS%%\RedBracket.plugin" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo ^(echo   ^<IsAutoLoad^>True^</IsAutoLoad^>^) ^>^> "%%CADMATE_PLUGINS%%\RedBracket.plugin" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo ^(echo   ^<IsLoadOnStartup^>True^</IsLoadOnStartup^>^) ^>^> "%%CADMATE_PLUGINS%%\RedBracket.plugin" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo ^(echo ^</Plugin^>^) ^>^> "%%CADMATE_PLUGINS%%\RedBracket.plugin" >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo. >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo. >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo ============================================= >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo  Installation Complete! >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo ============================================= >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo. >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo ✓ Plugin installed to: %%REDBRACKET_DIR%% >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo. >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo Next steps: >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo 1. Start CADMATE 2025 >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo 2. Look for the RedBracket tab in the ribbon >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo 3. If tab doesn't appear, type NETLOAD and browse to the DLL >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo. >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo For troubleshooting, see Documentation\README.txt >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo echo. >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"
echo pause >> "%PACKAGE_DIR%\Install_RedBracket_Plugin.bat"

:: Create uninstaller
echo Creating uninstaller...
echo @echo off > "%PACKAGE_DIR%\Uninstall_RedBracket_Plugin.bat"
echo title RedBracket CADMATE 2025 Plugin - Uninstallation >> "%PACKAGE_DIR%\Uninstall_RedBracket_Plugin.bat"
echo set "CADMATE_PLUGINS=%%APPDATA%%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns" >> "%PACKAGE_DIR%\Uninstall_RedBracket_Plugin.bat"
echo echo Removing RedBracket plugin... >> "%PACKAGE_DIR%\Uninstall_RedBracket_Plugin.bat"
echo rmdir /s /q "%%CADMATE_PLUGINS%%\RedBracket" 2^>nul >> "%PACKAGE_DIR%\Uninstall_RedBracket_Plugin.bat"
echo del "%%CADMATE_PLUGINS%%\RedBracket.plugin" 2^>nul >> "%PACKAGE_DIR%\Uninstall_RedBracket_Plugin.bat"
echo echo Plugin uninstalled. Restart CADMATE 2025 to complete removal. >> "%PACKAGE_DIR%\Uninstall_RedBracket_Plugin.bat"
echo pause >> "%PACKAGE_DIR%\Uninstall_RedBracket_Plugin.bat"

:: Create ZIP package
echo [5/5] Creating ZIP package...
powershell -command "Compress-Archive -Path '%PACKAGE_DIR%\*' -DestinationPath '%ZIP_FILE%' -Force"
if %errorLevel% neq 0 (
    echo ⚠ ZIP creation failed, but folder package is ready
) else (
    echo ✓ ZIP package created: %ZIP_FILE%
)

:: Clean up temporary directory
rmdir /s /q "%PACKAGE_DIR%"

echo.
echo =============================================
echo  Installation Package Created!
echo =============================================
echo.
echo Package file: %ZIP_FILE%
echo.
echo Contents:
echo ✓ Plugin DLL and dependencies
echo ✓ Automatic installer script
echo ✓ Uninstaller script
echo ✓ Documentation
echo.
echo To deploy:
echo 1. Copy the ZIP file to target machines
echo 2. Extract the ZIP file
echo 3. Right-click "Install_RedBracket_Plugin.bat" and "Run as administrator"
echo.
pause
