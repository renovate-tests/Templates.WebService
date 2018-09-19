Param ([string]$Version = "0.1-dev")
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

nuget pack -Version $Version -OutputDirectory artifacts -NoPackageAnalysis

popd
