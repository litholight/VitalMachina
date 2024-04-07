#!/bin/bash

# Set SDL2 library paths
export SDL2_LIB_PATH="/opt/homebrew/opt/sdl2/lib"
export SDL2_IMAGE_LIB_PATH="/opt/homebrew/opt/sdl2_image/lib"
export DYLD_LIBRARY_PATH="${SDL2_LIB_PATH}:${SDL2_IMAGE_LIB_PATH}:$DYLD_LIBRARY_PATH"

# Set debug flags
export DEBUG_SHOW_BOUNDING_BOXES=true

# Now run the project
dotnet run --project Mario/Mario.Native.MacOS/Mario.Native.MacOS.csproj
