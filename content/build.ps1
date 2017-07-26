Param ([Parameter(Mandatory=$True)][string]$Version)
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

.\src\build-dotnet.ps1 $Version
.\src\build-docker.ps1 $Version

# Inject version information into Docker Compose files
if (!(Test-Path artifacts)) { mkdir artifacts | Out-Null }
foreach ($x in (ls assets -Filter "*.yml"))
{
   (Get-Content $x.FullName -Encoding "UTF8").Replace('${DOCKER_REGISTRY}', $DockerRegistry).Replace('${VERSION}', $Version) | Out-File ("artifacts\" + $x.Name) -Encoding "UTF8"
}

popd
