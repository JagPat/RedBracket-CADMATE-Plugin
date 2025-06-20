# 🚨 Important Notice for Web Developers

## This is NOT a Web Application!

If you're a web developer who stumbled upon this project, **this is a C# desktop application** for AutoCAD/CADMATE, not a web application.

### What This Project Is:

- **C# .NET Framework 4.7.2** AutoCAD plugin
- **Windows desktop application** that runs inside AutoCAD/CADMATE
- **Binary plugin** (.dll file) that extends AutoCAD functionality
- **Enterprise software** for PLM (Product Lifecycle Management) integration

### What This Project Is NOT:

- ❌ Not a React/Vue/Angular web app
- ❌ Not a Node.js backend service
- ❌ Not a website or web API
- ❌ Not meant to run in a browser

### Why There's a package.json File:

The `package.json` file was added to make the development environment tools work, but this project doesn't actually use Node.js for its core functionality. The "dev server" simply displays information about how to properly build this C# project.

### If You Need to Work on This Project:

1. **You need Windows** - This won't build on Mac/Linux
2. **Install Visual Studio 2019+** - Not VS Code, the full Visual Studio IDE
3. **Install CADMATE 2025** - The target AutoCAD-compatible CAD software
4. **Read BUILD_INSTRUCTIONS.md** - Contains the actual build instructions

### If You're Looking for Web Development:

This is probably not the project you want. You might be looking for:

- A web frontend for this system (separate project)
- A REST API server (separate project)
- The RedBracket web interface (separate project)

### Need Help?

- For C# AutoCAD plugin development: See BUILD_INSTRUCTIONS.md
- For web development on RedBracket: Contact your team lead to find the correct repository
- For general questions: Ask your system administrator

---

**TL;DR**: This builds a Windows desktop plugin for AutoCAD. If you expected a website, you're in the wrong repository. 🏗️ ≠ 🌐
