# 🚀 RedBracket CADMATE 2025 Plugin - Complete Deployment Guide

## Overview

This guide will help you build, package, and deploy the RedBracket plugin for CADMATE 2025 to other Windows machines.

## 📋 Prerequisites

### Development Machine (Where you build)

- ✅ Windows 10/11 (64-bit)
- ✅ Visual Studio 2019 or later with ".NET desktop development" workload
- ✅ .NET Framework 4.7.2 Developer Pack
- ✅ CADMATE 2025 (for testing)

### Target Machines (Where you deploy)

- ✅ Windows 10/11 (64-bit)
- ✅ CADMATE 2025 (64-bit)
- ✅ .NET Framework 4.7.2 or later
- ✅ Administrator privileges (for installation)

## 🔧 Step 1: Build the Plugin

### 1.1 Verify Prerequisites

```bash
# Run this to check your build environment
Verify_Build_Prerequisites.bat
```

### 1.2 Build the Project

```bash
# This will build the entire solution
Build_RedBracket_Plugin.bat
```

**Expected Output:**

- ✅ `AutocadPlugIn\bin\x64\Release\RBAutocadPlugIn.dll` - Main plugin
- ✅ `AutocadPlugIn\bin\x64\Release\Newtonsoft.Json.dll` - JSON library
- ✅ `AutocadPlugIn\bin\x64\Release\RestSharp.dll` - HTTP client library
- ✅ Other dependencies and PDB files

### 1.3 Troubleshooting Build Issues

**Common Problems:**

1. **"MSBuild not found"**

   - Solution: Open "Developer Command Prompt for Visual Studio" and run the script from there

2. **"GRX SDK references not found"**

   - Ensure CADMATE 2025 is installed
   - Check the reference paths in `RBAutocadPlugIn.csproj`

3. **".NET Framework 4.7.2 not found"**
   - Download and install: [.NET Framework 4.7.2 Developer Pack](https://dotnet.microsoft.com/download/dotnet-framework/net472)

## 📦 Step 2: Create Installation Package

### 2.1 Package for Distribution

```bash
# This creates a ZIP file ready for deployment
Create_Installation_Package.bat
```

**Output:** `RedBracket_CADMATE2025_Plugin_v1.5.0.zip`

### 2.2 Package Contents

- 📁 `Plugin/` - All DLL files and dependencies
- 📁 `Documentation/` - User guides and README files
- 📄 `Install_RedBracket_Plugin.bat` - Automatic installer
- 📄 `Uninstall_RedBracket_Plugin.bat` - Removal tool

## 🚀 Step 3: Deploy to Target Machines

### 3.1 Copy Installation Package

1. Transfer `RedBracket_CADMATE2025_Plugin_v1.5.0.zip` to target machine
2. Extract to a temporary folder (e.g., Desktop)

### 3.2 Install Plugin

1. **Right-click** `Install_RedBracket_Plugin.bat`
2. Select **"Run as administrator"**
3. Follow the installation prompts

### 3.3 Installation Location

The plugin will be installed to:

```
%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket\
```

## 🧪 Step 4: Test Installation

### 4.1 Automated Testing

```bash
# Run this on the target machine after installation
Test_Plugin_Installation.bat
```

### 4.2 Manual Testing in CADMATE

1. **Start CADMATE 2025**
2. **Check for RedBracket Tab:**

   - Look for "RedBracket" tab in the ribbon
   - If missing, continue to step 3

3. **Manual Loading (if needed):**

   ```
   Command: NETLOAD
   Browse to: %APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket\RBAutocadPlugIn.dll
   Click: Load
   ```

4. **Verify Functionality:**
   - Check that RedBracket tab appears
   - Test basic commands (Connect, Browse, etc.)
   - Verify no error messages in command line

## 🔧 Step 5: Troubleshooting Deployment

### 5.1 Plugin Not Loading

**Symptoms:** RedBracket tab doesn't appear

**Solutions:**

1. **Check .NET Framework:**

   ```bash
   # Run on target machine
   reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" /v Release
   ```

   Should show value ≥ 461808 (for .NET 4.7.2+)

2. **Manual Load Test:**

   - In CADMATE: `NETLOAD`
   - Browse to plugin DLL
   - Watch for error messages

3. **Check File Permissions:**
   - Ensure plugin files aren't blocked
   - Right-click DLL → Properties → Unblock if needed

### 5.2 Connection Issues

**Symptoms:** Plugin loads but can't connect to RedBracket

**Solutions:**

1. **Network Access:** Ensure target machine can reach RedBracket server
2. **Firewall:** Check Windows Firewall and corporate firewalls
3. **Proxy Settings:** Configure if behind corporate proxy

### 5.3 Compatibility Issues

**Symptoms:** Plugin crashes or behaves unexpectedly

**Solutions:**

1. **CADMATE Version:** Ensure CADMATE 2025 R24.2 or compatible
2. **SDK Compatibility:** Check GRX SDK version matches CADMATE installation
3. **Dependencies:** Verify all DLL dependencies are present

## 📁 File Structure Reference

### Build Output Structure:

```
AutocadPlugIn/bin/x64/Release/
├── RBAutocadPlugIn.dll        # Main plugin
├── RBAutocadPlugIn.pdb        # Debug symbols
├── Newtonsoft.Json.dll        # JSON handling
├── RestSharp.dll              # HTTP client
├── ExpandableGridView.dll     # UI component
├── IOM.dll                    # Helper library
├── acad.lsp                   # AutoLISP script
└── start.scr                  # Startup script
```

### Installation Structure:

```
%APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns/
├── RedBracket/                # Plugin files folder
│   ├── RBAutocadPlugIn.dll
│   ├── Newtonsoft.Json.dll
│   ├── RestSharp.dll
│   └── [other dependencies]
└── RedBracket.plugin          # Plugin loader config
```

## 🔐 Security Considerations

1. **Code Signing:** Consider signing the DLL for enterprise deployment
2. **Admin Rights:** Installation requires admin privileges
3. **Network Security:** Plugin communicates with RedBracket servers
4. **File Permissions:** Ensure proper access rights on target machines

## 📞 Support and Maintenance

### Version Information

- **Plugin Version:** 1.5.0
- **CADMATE Target:** 2025 R24.2
- **GRX SDK Version:** 20250605
- **.NET Framework:** 4.7.2

### For Technical Support

1. Collect error messages from CADMATE command line
2. Check Windows Event Viewer for .NET Runtime errors
3. Run `Test_Plugin_Installation.bat` for diagnostic information
4. Document exact steps that lead to issues

### Updating the Plugin

To update to a new version:

1. Run `Uninstall_RedBracket_Plugin.bat` on target machines
2. Build and package new version
3. Deploy using the same installation process
4. Test functionality after update

---

## 🎉 Success Checklist

After deployment, verify:

- ✅ RedBracket tab appears in CADMATE ribbon
- ✅ Connection to RedBracket server works
- ✅ File operations (save/open) function correctly
- ✅ No error messages in command line
- ✅ Plugin persists after CADMATE restart

## 📋 Quick Reference Commands

| Task                    | Command                           |
| ----------------------- | --------------------------------- |
| Check build environment | `Verify_Build_Prerequisites.bat`  |
| Build plugin            | `Build_RedBracket_Plugin.bat`     |
| Create installer        | `Create_Installation_Package.bat` |
| Test installation       | `Test_Plugin_Installation.bat`    |
| Manual load in CADMATE  | `NETLOAD` → Browse to DLL         |
| Check plugin status     | `APPLOAD` in CADMATE              |

---

_This guide ensures proper deployment of RedBracket plugin while maintaining all existing RedBracket endpoints and functionality._
