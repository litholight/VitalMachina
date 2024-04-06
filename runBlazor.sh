#!/bin/bash

# Set SDL2 library paths
export SDL2_LIB_PATH="/opt/homebrew/opt/sdl2/lib"
export SDL2_IMAGE_LIB_PATH="/opt/homebrew/opt/sdl2_image/lib"
export DYLD_LIBRARY_PATH="${SDL2_LIB_PATH}:${SDL2_IMAGE_LIB_PATH}:$DYLD_LIBRARY_PATH"

dotnet run --project Mario/Mario.Blazor/Mario.csproj