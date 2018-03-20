Param ([string]$Version = "0.1-pre", [Switch]$DeployLocal)
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

src\build-dotnet.ps1 $Version
src\test-dotnet.ps1
src\build-docker.ps1 $Version
release\build.ps1 $Version

if ($DeployLocal) {
    ax deploy -f deploy\local-dev.yml --feed "$(Resolve-Path release\asset-$Version.xml)=$Version"
}

popd
