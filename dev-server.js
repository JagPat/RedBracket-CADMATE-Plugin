const http = require("http");

const html = `<!DOCTYPE html>
<html>
<head>
    <title>RedBracket AutoCAD Plugin</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            max-width: 800px;
            margin: 50px auto;
            padding: 20px;
            background: #f5f5f5;
            line-height: 1.6;
        }
        h1 {
            color: #d32f2f;
        }
        h2 {
            color: #333;
            border-bottom: 2px solid #ddd;
            padding-bottom: 5px;
        }
        code {
            background: #e8e8e8;
            padding: 2px 4px;
            border-radius: 3px;
            font-family: 'Courier New', monospace;
        }
        .warning {
            background: #fff3cd;
            border: 1px solid #ffeaa7;
            padding: 15px;
            border-radius: 5px;
            margin: 20px 0;
        }
        .info {
            background: #d1ecf1;
            border: 1px solid #bee5eb;
            padding: 15px;
            border-radius: 5px;
            margin: 20px 0;
        }
        ul, ol {
            margin-left: 20px;
        }
        li {
            margin: 5px 0;
        }
    </style>
</head>
<body>
    <h1>🚨 RedBracket AutoCAD Plugin</h1>
    
    <div class="warning">
        <strong>This is NOT a web application!</strong><br>
        This is a C# desktop plugin for AutoCAD/CADMATE.
    </div>

    <div class="info">
        <strong>Note:</strong> This web server exists only to satisfy the development environment proxy. 
        The actual project is built using Visual Studio/MSBuild.
    </div>

    <h2>📋 Project Information</h2>
    <ul>
        <li><strong>Type:</strong> C# .NET Framework 4.7.2 AutoCAD Plugin</li>
        <li><strong>Target:</strong> CADMATE 2025 with GRX SDK</li>
        <li><strong>Platform:</strong> Windows x64</li>
        <li><strong>Output:</strong> Binary DLL plugin for AutoCAD</li>
    </ul>

    <h2>🔧 To Build This Project</h2>
    <ol>
        <li>Use <strong>Windows 10+</strong> (64-bit) - This won't build on Mac/Linux</li>
        <li>Install <strong>Visual Studio 2019+</strong> (not VS Code)</li>
        <li>Install <strong>CADMATE 2025</strong> with GRX SDK</li>
        <li>Install <strong>.NET Framework 4.7.2 Developer Pack</strong></li>
        <li>Open <code>CADPlugIns.sln</code> in Visual Studio</li>
        <li>Select <strong>Release</strong> configuration and <strong>x64</strong> platform</li>
        <li>Build → Build Solution (F6)</li>
    </ol>

    <h2>📁 Output Location</h2>
    <p>Built files will be in: <code>AutocadPlugIn/bin/x64/Release/</code></p>

    <h2>📚 Documentation</h2>
    <ul>
        <li><code>BUILD_INSTRUCTIONS.md</code> - Detailed build instructions</li>
        <li><code>WEB_DEVELOPERS_README.md</code> - Info for web developers</li>
        <li><code>README.md</code> - General project information</li>
    </ul>

    <h2>🎯 What This Plugin Does</h2>
    <p>This plugin integrates AutoCAD/CADMATE with the RedBracket PLM (Product Lifecycle Management) system, 
    allowing users to manage CAD files, check in/out drawings, and maintain version control within the CAD environment.</p>

    <hr>
    <p><em>Development server running on port 3000 for proxy compatibility.</em></p>
</body>
</html>`;

const server = http.createServer((req, res) => {
  res.writeHead(200, {
    "Content-Type": "text/html",
    "Cache-Control": "no-cache",
    "Access-Control-Allow-Origin": "*",
  });
  res.end(html);
});

const PORT = 3000;
server.listen(PORT, () => {
  console.log(`📄 RedBracket AutoCAD Plugin Documentation Server`);
  console.log(`🌐 Running on http://localhost:${PORT}`);
  console.log(`📝 This serves project documentation for the C# AutoCAD plugin`);
});
