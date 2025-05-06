@echo off
REM COPIED FROM https://github.com/mediwareinc/Atlas.BuildScript/blob/master/Samples/build.bat - DO NOT MODIFY IN CONSUMER; FIX BUGS IN Atlas.BuildScript

REM This is just a bootstrap file for loading the main Atlas.BuildScript scripts
REM You shouldn't need to change this file.
REM See https://github.com/mediwareinc/Atlas.BuildScript for more details

:: Define The version of the build package we want to use.
set package=Atlas.BuildScript
set package_version=2.0.0


:: Download NuGet
set builddir=%~dp0\build
if not exist "%builddir%\tools" mkdir "%builddir%\tools"
if not exist "%builddir%\tools\nuget" mkdir "%builddir%\tools\nuget"
if not exist "%builddir%\tools\nuget\nuget.exe" powershell -NonInteractive -NoProfile -ExecutionPolicy Unrestricted -Command "(New-Object Net.WebClient).DownloadFile('https://dist.nuget.org/win-x86-commandline/latest/nuget.exe', '%builddir%\tools\nuget\nuget.exe')"

:: Download Atlas.Build package
set azure_stable=https://pkgs.dev.azure.com/mediwareis/_packaging/WellSky-Release/nuget/v3/index.json
set azure_prerelease=https://pkgs.dev.azure.com/mediwareis/_packaging/WellSky-PreRelease/nuget/v3/index.json

:: Select the azure feed based on if the version has a prerelease tag (contains a '-')
if x%package_version:-=%==x%package_version% (SET azure_feed=%azure_stable%) ELSE (SET azure_feed=%azure_prerelease%)

if not exist "%builddir%\tools\%package%" %builddir%\tools\nuget\nuget.exe install %package% -Source %azure_feed% -Version %package_version% -NonInteractive -OutputDirectory %builddir%\tools -ExcludeVersion

:: Run Atlas.Build
powershell -NonInteractive -NoProfile -ExecutionPolicy Unrestricted -Command "& { %builddir%\tools\%package%\Content\build.ps1 -Script %builddir%\build.cake %* }"
