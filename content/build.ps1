Param ([Parameter(Mandatory=$True)][string]$Version)
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

$DockerRegistry = if ($Version.Contains("-")) {"docker-ci.axoom.cloud"} else {"docker.axoom.cloud"}

pushd src

# Build source
.\build.ps1 $Version

# Build tagged Docker images
$env:DOCKER_REGISTRY = $DockerRegistry
$env:VERSION = $Version
docker-compose -f docker-compose.yml build

popd

# Inject version information into Docker Compose files
if (!(Test-Path artifacts)) { mkdir artifacts | Out-Null }
foreach ($x in (ls assets -Filter "*.yml"))
{
   (Get-Content $x.FullName -Encoding "UTF8").Replace('${DOCKER_REGISTRY}', $DockerRegistry).Replace('${VERSION}', $Version) | Out-File ("artifacts\" + $x.Name) -Encoding "UTF8"
}

popd
