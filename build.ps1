Param ([Parameter(Mandatory=$True)][string]$Version)
$ErrorActionPreference = "Stop"

nuget pack -Version $Version -OutputDirectory artifacts -NoPackageAnalysis
