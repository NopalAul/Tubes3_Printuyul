#!/bin/bash

# Start the first dotnet project in a new Terminal tab
osascript -e 'tell application "Terminal" to do script "cd \"$(pwd)\" && dotnet run --project src/FingerprintApi"'

# Start the second dotnet project in a new Terminal tab
osascript -e 'tell application "Terminal" to do script "cd \"$(pwd)\" && dotnet run --project src/newjeans_avalonia"'
