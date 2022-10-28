#!/bin/bash
rm -rf build
mkdir build

dotnet build -c Release -o ./build ./src/GerrKoff.Monitoring/GerrKoff.Monitoring.csproj
