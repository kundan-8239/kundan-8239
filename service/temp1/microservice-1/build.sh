#!/usr/bin/env bash

# COPIED FROM https://github.com/mediwareinc/Atlas.BuildScript/blob/master/Samples/build.sh - DO NOT MODIFY IN CONSUMER; FIX BUGS IN Atlas.BuildScript

set -euo pipefail

# This is just a bootstrap file for loading the main Atlas.BuildScript scripts
# You shouldn't need to change this file.
# See https://github.com/mediwareinc/Atlas.BuildScript for more details

# Define The version of the build package we want to use.
PACKAGE=Atlas.BuildScript
PACKAGE_VERSION=2.0.0

# Define directories.
SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
TOOLS_DIR=$SCRIPT_DIR/Build/tools
INSTALL_DIR="$TOOLS_DIR/$PACKAGE"
INSTALL_DIR_TMP="$TOOLS_DIR/${PACKAGE}-tmp"

# Make sure the tools folder exist.
if [ ! -d "$TOOLS_DIR" ]; then
  mkdir "$TOOLS_DIR"
fi

# Download the Atlas.Build package
AZURE_STABLE=https://pkgs.dev.azure.com/mediwareis/_packaging/WellSky-Release/nuget/v3/index.json
AZURE_PRERELEASE=https://pkgs.dev.azure.com/mediwareis/_packaging/WellSky-PreRelease/nuget/v3/index.json

# Select the AZURE feed based on if the version has a prerelease tag (contains a '-')
if [[ "$PACKAGE_VERSION" == *-* ]]; then
  AZURE_FEED="$AZURE_PRERELEASE"
else
  AZURE_FEED="$AZURE_STABLE"
fi

if [ ! -d "$TOOLS_DIR/$PACKAGE" ]; then

  # There isn't a good solution for nuget.exe on linux.  We know the dotnet sdk is installed so let's just use that.
  # We need a project of some kind just so we can use `dotnet add package`
  echo ":: Creating temp classlib to support nuget install..."
  FAKE_PROJECT_DIR=$TOOLS_DIR/fake-project-for-nuget
  dotnet new classlib --no-restore --force -o $FAKE_PROJECT_DIR

  # Now we add the package to our fake library.  We point the package directory where we want the package to end up
  echo ":: Installing package $PACKAGE v$PACKAGE_VERSION..."
  dotnet add $FAKE_PROJECT_DIR package --source $AZURE_FEED --version $PACKAGE_VERSION --package-directory $INSTALL_DIR_TMP $PACKAGE

  # The above installs to a path with the version number as a folder in the path.  We want it without the version number but `dotnet add`
  # can't do that so we move things around manually.  The package name is being lowercased so we have to use ${PACKAGE,,} to lowercase
  # our input variable.
  echo ":: Updating package install location..."
  mv $INSTALL_DIR_TMP/${PACKAGE,,}/${PACKAGE_VERSION,,} $INSTALL_DIR
  rm -r $INSTALL_DIR_TMP
  rm -r $FAKE_PROJECT_DIR
fi

# Make sure that Atlas.Build has been installed.
ATLAS_BOOTSTRAP="$INSTALL_DIR/Content/build.sh"
if [ ! -f "$ATLAS_BOOTSTRAP" ]; then
    echo "Could not find Atlas bootstrap at '$ATLAS_BOOTSTRAP'."
    exit 1
fi

# Run the Atlas Bootstrap
printf "Running script: %s\n" $SCRIPT_DIR

# chmod is needed below because when running in azure you are not running as root
# And "unzip" doesn't set the permissions for you.
chmod -R u+rw,go+rw "$TOOLS_DIR"
bash "$ATLAS_BOOTSTRAP" "$SCRIPT_DIR/Build/build.cake" "$@"