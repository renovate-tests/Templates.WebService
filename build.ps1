Param ([Parameter(Mandatory=$True)][string]$Version)
$ErrorActionPreference = "Stop"

nuget pack -NoPackageAnalysis -Version $Version
