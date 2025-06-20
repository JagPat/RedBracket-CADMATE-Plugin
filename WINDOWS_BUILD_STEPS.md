# 🖥️ Windows Machine Build Steps

## 📂 **After File Transfer**

1. **Open Command Prompt as Administrator**
   - Press `Win + X`
   - Select "Command Prompt (Admin)" or "PowerShell (Admin)"
   - Navigate to your project folder: `cd C:\path\to\your\RedBracket_Plugin`

## 🔧 **Build Process (Run in Order)**

### **Step 1: Verify Environment**

```cmd
Verify_Build_Prerequisites.bat
```

**Expected Output:**

- ✅ Windows 10/11 detected
- ✅ 64-bit system detected
- ✅ Visual Studio found
- ✅ .NET Framework 4.7.2+ installed
- ✅ CADMATE 2025 installation found
- ✅ MSBuild found

**If any ❌ appear:** Install the missing components first

### **Step 2: Build Plugin**

```cmd
Build_RedBracket_Plugin.bat
```

**Expected Output:**

- ✅ MSBuild found
- ✅ Packages restored
- ✅ Build successful
- ✅ RBAutocadPlugIn.dll created

**Build Location:** `AutocadPlugIn\bin\x64\Release\`

### **Step 3: Create Deployment Package**

```cmd
Create_Installation_Package.bat
```

**Expected Output:**

- ✅ Package directory created
- ✅ Plugin files copied
- ✅ Installer script created
- ✅ ZIP package created: `RedBracket_CADMATE2025_Plugin_v1.5.0.zip`

### **Step 4: Test Installation (Optional)**

```cmd
Test_Plugin_Installation.bat
```

## 🎯 **Success Indicators**

**After successful build, you should have:**

- ✅ `RedBracket_CADMATE2025_Plugin_v1.5.0.zip` (deployment package)
- ✅ No error messages during build
- ✅ DLL files in `AutocadPlugIn\bin\x64\Release\`

## 🚀 **Deploy to Other Machines**

1. **Copy the ZIP file** to target machines
2. **Extract and run as Admin:** `Install_RedBracket_Plugin.bat`
3. **Start CADMATE 2025** and verify RedBracket tab appears

## 🚨 **Common Issues & Solutions**

### Issue: "MSBuild not found"

**Solution:** Open "Developer Command Prompt for Visual Studio" instead

### Issue: "GRX SDK references not found"

**Solution:**

- Ensure CADMATE 2025 is installed
- Check paths in `RBAutocadPlugIn.csproj`

### Issue: ".NET Framework not found"

**Solution:** Install .NET Framework 4.7.2 Developer Pack

### Issue: "Build failed with errors"

**Solution:** Open Visual Studio and build manually to see detailed errors

## ⏱️ **Expected Timeline**

- File transfer: 5-10 minutes
- Prerequisite verification: 2 minutes
- Build process: 5-10 minutes
- Package creation: 2-3 minutes
- **Total: ~15-25 minutes**

## 📞 **Success Confirmation**

**When everything works, you'll see:**

```
=============================================
Build Successful!
=============================================

Package created successfully!
File: RedBracket_CADMATE2025_Plugin_v1.5.0.zip
```

**This ZIP file contains everything needed for deployment! 🎉**
