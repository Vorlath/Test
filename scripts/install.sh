#!/bin/bash

# ---------------
# BEGIN VARIABLES
# ---------------

MONOGAME_BUILD_VERSION="1.3.3.7-cpt"

WORKING_DIRECTORY=$(pwd)
SCRIPT_DIR=$(dirname "$(realpath "$0")")
MONOGAME_DIRECTORY=$(realpath "$SCRIPT_DIR/../libraries/MonoGame")


# Default flag value
GIT_UPDATE=true
BUILD_TOOLS=true

# Parse CLI arguments for a specific flag (e.g., --bypass)
for arg in "$@"; do
  if [[ "$arg" == "--no-git-update" ]]; then
    GIT_UPDATE=false
  elif [[ "$arg" == "--no-build" ]]; then
    BUILD_TOOLS=false
  fi
done

# Update submodules
if [ "$GIT_UPDATE" = true ]; then
    cd "$SCRIPT_DIR/.."
    git submodule update --init --recursive
fi

# ---------------------------
# BEGIN MONOGAME INSTALLATION
# ---------------------------

# Build MonoGame

if [ "$BUILD_TOOLS" = true ]; then
    cd "$MONOGAME_DIRECTORY"

    if [[ -f "./build.cake" ]]; then
        dotnet tool restore
        dotnet cake --build-version "$MONOGAME_BUILD_VERSION"

        MONOGAME_BUILD_VERSION="${MONOGAME_BUILD_VERSION}-develop"
    else
        dotnet run --project "$MONOGAME_DIRECTORY/build/Build.csproj" -- "--build-version" "$MONOGAME_BUILD_VERSION"

        # Why isn't this included in Build.csproj tho?
        dotnet pack -o "$MONOGAME_DIRECTORY/artifacts/NuGet" \
            /p:Version="$MONOGAME_BUILD_VERSION" \
            "$MONOGAME_DIRECTORY/Tools/MonoGame.Content.Builder.Editor.Launcher/MonoGame.Content.Builder.Editor.Launcher.Linux.csproj"
    fi

    git reset --hard HEAD
fi

cd "$WORKING_DIRECTORY"

# Uninstall old tools (if any)
if dotnet tool list | grep -q "dotnet-mgcb-editor-linux"; then
    dotnet tool uninstall dotnet-mgcb-editor-linux
fi

if dotnet tool list | grep -q "dotnet-mgcb-editor"; then
    dotnet tool uninstall dotnet-mgcb-editor
fi

if dotnet tool list | grep -q "dotnet-mgcb"; then
    dotnet tool uninstall dotnet-mgcb
fi

# Install new tools
# Build.csproj uses a special package version in github workflows. By ditching the explicit version on the install we can unsure whatever
# version workflows decides to use will be utilized

#dotnet tool install --version "$MONOGAME_BUILD_VERSION" --add-source "$MONOGAME_DIRECTORY/artifacts/NuGet" dotnet-mgcb
#dotnet tool install --version "$MONOGAME_BUILD_VERSION" --add-source "$MONOGAME_DIRECTORY/artifacts/NuGet" dotnet-mgcb-editor-linux
#dotnet tool install --version "$MONOGAME_BUILD_VERSION" --add-source "$MONOGAME_DIRECTORY/artifacts/NuGet" dotnet-mgcb-editor
dotnet tool install --local --create-manifest-if-needed --add-source "$MONOGAME_DIRECTORY/artifacts/NuGet" dotnet-mgcb
dotnet tool install --local --create-manifest-if-needed --add-source "$MONOGAME_DIRECTORY/artifacts/NuGet" dotnet-mgcb-editor-linux
dotnet tool install --local --create-manifest-if-needed --add-source "$MONOGAME_DIRECTORY/artifacts/NuGet" dotnet-mgcb-editor