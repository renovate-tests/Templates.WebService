Param ([string]$Version = "0.1-dev")
$ErrorActionPreference = "Stop"
pushd $PSScriptRoot

nuget pack -Version $Version -OutputDirectory artifacts -NoPackageAnalysis

popd
