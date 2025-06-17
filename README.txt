RedBracket CADMATE 2025 Plugin
==============================

This package contains the RedBracket integration plugin for CADMATE 2025.

System Requirements
------------------
- Windows 10/11 (64-bit)
- CADMATE 2025 (64-bit)
- .NET Framework 4.7.2 or later

Installation Instructions
------------------------

1. **Prerequisites**
   - Ensure CADMATE 2025 is installed
   - Close CADMATE if it's running

2. **Installation**
   - Copy the entire RedBracket folder to your CADMATE 2025 plugins directory:
     `%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\`
   - OR run the `Install_RedBracket_Plugin.bat` as Administrator

3. **First Run**
   - Start CADMATE 2025
   - The RedBracket ribbon tab should appear automatically
   - If not, type `NETLOAD` in the command line and browse to:
     `%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket\RBAutocadPlugIn.dll`

4. **Verification**
   - Look for the RedBracket tab in the ribbon
   - Check for any error messages in the command line

Troubleshooting
---------------

1. **Plugin not loading**
   - Run CADMATE as Administrator
   - Check if all files were copied to the correct location
   - Verify .NET Framework 4.7.2 or later is installed

2. **Missing ribbon tab**
   - Type `RIBBON` in the command line
   - Right-click on the ribbon and ensure "RedBracket" is checked

3. **Error messages**
   - Note any error messages that appear
   - Check the Windows Event Viewer for .NET Runtime errors

Uninstallation
--------------
1. Close CADMATE 2025
2. Delete the RedBracket folder from:
   `%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\`
3. Delete the RedBracket.plugin file from the PlugIns folder

Support
-------
For assistance, please contact support@redbracket.com

Version: 1.0.0
Last Updated: June 16, 2025
