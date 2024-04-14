#!/bin/bash

# Identify the machine
MACHINE_TYPE=$(uname -m)

if [ "${MACHINE_TYPE}" == "x86_64" ]; then
    # Assume iMac uses Intel architecture
    export SDL2_TTF_PATH="/usr/local/opt/sdl2_ttf/lib/libSDL2_ttf.dylib"
elif [ "${MACHINE_TYPE}" == "arm64" ]; then
    # Assume MacBook Air uses Apple Silicon
    export SDL2_TTF_PATH="/opt/homebrew/Cellar/sdl2_ttf/2.22.0/lib/libSDL2_ttf-2.0.0.dylib"
fi

# Set SDL2 library paths
export SDL2_LIB_PATH="/opt/homebrew/opt/sdl2/lib"
export SDL2_IMAGE_LIB_PATH="/opt/homebrew/opt/sdl2_image/lib"

# Update DYLD_LIBRARY_PATH with SDL2_TTF_PATH
export DYLD_LIBRARY_PATH="${SDL2_LIB_PATH}:${SDL2_IMAGE_LIB_PATH}:${SDL2_TTF_PATH}:$DYLD_LIBRARY_PATH"

# Set debug flags
export DEBUG_SHOW_BOUNDING_BOXES=true
export DEBUG_SHOW_POINTER_COORDINATES=true  # If you want this for Blazor as well

# Now run the project
dotnet run --project Mario/Mario.Blazor/Mario.csproj
