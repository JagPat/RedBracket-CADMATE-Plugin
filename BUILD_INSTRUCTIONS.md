# CADMATE 2025 Integration - Build Instructions

This document provides step-by-step instructions for setting up the build environment and building the RedBracket CAD plugin for CADMATE 2025.

## Prerequisites

### Hardware Requirements
- Windows 10 or later (64-bit)
- Minimum 8GB RAM (16GB recommended)
- 10GB free disk space

### Software Requirements
1. **CADMATE 2025**
   - Must be installed on the build machine
   - Ensure the GRX SDK is installed (typically comes with CADMATE 2025 installation)

2. **Development Tools**
   - [Visual Studio 2019 or later](https://visualstudio.microsoft.com/downloads/)
     - Workload: ".NET desktop development"
     - Individual components: ".NET Framework 4.7.2 targeting pack"
   - [.NET Framework 4.7.2 Developer Pack](https://dotnet.microsoft.com/download/dotnet-framework/net472)

3. **Source Code**
   - Clone the repository to your local machine
   - Switch to the `feature/cadmate-2025-upgrade` branch

## Build Instructions

1. **Open the Solution**
   - Launch Visual Studio 2019 or later
   - Open `CADPlugIns.sln` from the root directory

2. **Restore NuGet Packages**
   - Right-click the solution in Solution Explorer
   - Select "Restore NuGet Packages"

3. **Build the Solution**
   - Select "Release" configuration
   - Select "x64" platform
   - Build > Build Solution (or press F6)

## Deployment

1. **Output Files**
   - The built plugin files will be in: 
     `AutocadPlugIn\bin\x64\Release\`

2. **Installation**
   - Copy the following files to your CADMATE 2025 installation's plugin directory:
     - `RBAutocadPlugIn.dll`
     - `RBAutocadPlugIn.pdb` (optional, for debugging)
     - `Newtonsoft.Json.dll`
     - `RestSharp.dll`
     - Any other dependency DLLs

## Troubleshooting

### Common Build Issues

1. **Missing .NET Framework 4.7.2**
   - Error: "The reference assemblies for .NETFramework,Version=v4.7.2 were not found"
   - Solution: Install the .NET Framework 4.7.2 Developer Pack

2. **Missing GRX SDK References**
   - Error: "Cannot find reference to GcCoreMgd.dll or other GRX SDK assemblies"
   - Solution: Ensure CADMATE 2025 with GRX SDK is installed and update the reference paths in the project

3. **Platform Mismatch**
   - Error: "Platform not found" or similar
   - Solution: Ensure you've selected "x64" as the platform and "Release" configuration

## Testing

1. **Load the Plugin in CADMATE 2025**
   - Start CADMATE 2025
   - Use the `NETLOAD` command to load the plugin
   - Verify the RedBracket ribbon tab appears

2. **Basic Functionality Tests**
   - Test connection to RedBracket
   - Verify file operations (open/save)
   - Test all ribbon commands

## Support

For additional support, please contact your system administrator or refer to the GRX SDK documentation included with your CADMATE 2025 installation.
