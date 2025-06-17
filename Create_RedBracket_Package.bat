@echo off
title Create RedBracket CADMATE 2025 Package
echo =============================================
echo  Creating RedBracket CADMATE 2025 Package
echo =============================================

:: Set paths
set "SOURCE_DIR=%~dp0"
set "OUTPUT_DIR=%SOURCE_DIR%RedBracket_CADMATE2025_Plugin"
set "ZIP_FILE=RedBracket_CADMATE2025_Plugin.zip"

:: Create output directory
echo Creating package directory...
if exist "%OUTPUT_DIR%" rmdir /s /q "%OUTPUT_DIR%"
mkdir "%OUTPUT_DIR%"

:: Copy files
echo Copying files...

:: Main plugin files
xcopy /Y /E /I "%SOURCE_DIR%AutocadPlugIn\bin\x64\Release\*.dll" "%OUTPUT_DIR%\"
xcopy /Y /E /I "%SOURCE_DIR%AutocadPlugIn\bin\x64\Release\*.pdb" "%OUTPUT_DIR%\"

:: Copy GRX SDK files
xcopy /Y /E /I "%SOURCE_DIR%grxsdk\inc\*.dll" "%OUTPUT_DIR%\"

:: Copy documentation and installers
copy /Y "%SOURCE_DIR%README.txt" "%OUTPUT_DIR%\"
copy /Y "%SOURCE_DIR%Install_RedBracket_Plugin.bat" "%OUTPUT_DIR%\"

:: Create ZIP archive
echo Creating ZIP archive...
powershell -command "Compress-Archive -Path '%OUTPUT_DIR%\*' -DestinationPath '%SOURCE_DIR%%ZIP_FILE%' -Force"

:: Cleanup
rmdir /s /q "%OUTPUT_DIR%"

echo.
echo =============================================
echo  Package created successfully!
echo  File: %ZIP_FILE%
echo =============================================
echo.
pause
