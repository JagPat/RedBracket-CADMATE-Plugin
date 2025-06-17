#!/bin/bash

# Create main directories
mkdir -p "src/AutocadPlugIn/Commands"
mkdir -p "src/AutocadPlugIn/Controller"
mkdir -p "src/AutocadPlugIn/UI Forms"
mkdir -p "docs"
mkdir -p "lib"
mkdir -p "tests"

# Create placeholder files
touch "src/AutocadPlugIn/Commands/.gitkeep"
touch "src/AutocadPlugIn/Controller/.gitkeep"
touch "src/AutocadPlugIn/UI Forms/.gitkeep"
touch "docs/.gitkeep"
touch "lib/.gitkeep"
touch "tests/.gitkeep"

echo "Project structure created successfully!"
