#!/bin/sh
set -e

echo "Starting container..."

exec dotnet ECE.Api.dll
