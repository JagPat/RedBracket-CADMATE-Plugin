# 🚨 IMMEDIATE NEXT STEPS - RedBracket CADMATE 2025 Plugin

## 🖥️ **Current Situation**

You're currently on a **Linux/Unix environment**, but the RedBracket AutoCAD plugin **must be built on Windows**.

## 🔄 **What You Need to Do Right Now**

### **Step 1: Transfer to Windows Machine**

**Copy these files to a Windows machine with CADMATE 2025:**

#### 📁 **Essential Folders:**

```
✓ AutocadPlugIn/          # All source code
✓ grxsdk/                 # GRX SDK for CADMATE 2025
✓ Binaries/               # Dependencies (RestSharp, Newtonsoft.Json, etc.)
✓ RedBracketConnector/    # Connector library
✓ Help/                   # Documentation
```

#### 📄 **Essential Files:**

```
✓ CADPlugIns.sln                    # Visual Studio solution
✓ *.bat                             # All deployment scripts I created
✓ DEPLOYMENT_GUIDE.md               # Complete instructions
✓ BUILD_INSTRUCTIONS.md             # Build documentation
✓ README.txt                        # User documentation
```

### **Step 2: Windows Prerequisites**

**On the Windows machine, ensure you have:**

- ✅ **Windows 10/11 (64-bit)**
- ✅ **Visual Studio 2019 or later** ([Download here](https://visualstudio.microsoft.com/downloads/))
  - Include ".NET desktop development" workload
- ✅ **.NET Framework 4.7.2 Developer Pack** ([Download here](https://dotnet.microsoft.com/download/dotnet-framework/net472))
- ✅ **CADMATE 2025 (64-bit)** - Must be installed

### **Step 3: Build Process on Windows**

**Once on Windows machine, run these commands in order:**

```bash
# 1. Check prerequisites
Verify_Build_Prerequisites.bat

# 2. Build the plugin
Build_RedBracket_Plugin.bat

# 3. Create deployment package
Create_Installation_Package.bat

# 4. Test installation (optional)
Test_Plugin_Installation.bat
```

## 🎯 **Expected Results**

After successful build:

- ✅ `RedBracket_CADMATE2025_Plugin_v1.5.0.zip` will be created
- ✅ This ZIP contains everything needed for deployment
- ✅ You can deploy this ZIP to any Windows machine with CADMATE 2025

## 🚨 **Alternative: Use Existing EXE**

**If you already have a working EXE from previous integration:**

1. **Check if it's compatible:**

   - Does it work with CADMATE 2025?
   - Are RedBracket endpoints still working?

2. **If it works, you might just need to:**

   - Copy it to the correct plugin directory
   - Create the plugin configuration file
   - Test functionality

3. **Location for existing plugin:**
   ```
   %APPDATA%\CADMATE\CADMATE 2025\R24.2\enu\PlugIns\RedBracket\
   ```

## 📞 **What to Do Next**

**Choose your path:**

### **Path A: Build from Source (Recommended)**

1. Transfer files to Windows machine
2. Follow the build process above
3. Deploy the new package

### **Path B: Test Existing EXE**

1. Copy existing EXE to Windows machine with CADMATE 2025
2. Try manual installation
3. Test functionality

## 🤔 **Questions for You**

To help you better, please let me know:

1. **Do you have access to a Windows machine with CADMATE 2025?**
2. **Do you want to try the existing EXE first, or build from source?**
3. **Are there any specific compatibility issues you've encountered before?**

## 📋 **Quick Transfer Checklist**

**Before transferring to Windows:**

- [ ] All source files copied
- [ ] All .bat scripts copied
- [ ] DEPLOYMENT_GUIDE.md copied
- [ ] grxsdk folder copied
- [ ] Binaries folder copied

**On Windows machine:**

- [ ] Visual Studio installed
- [ ] CADMATE 2025 installed
- [ ] .NET Framework 4.7.2+ installed
- [ ] All files transferred successfully

---

**Once you're on a Windows machine, the deployment process will be smooth and automated! 🚀**
