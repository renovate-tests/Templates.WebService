Param ([Parameter(Mandatory=$True)][string]$Version)
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

$dockerRegistry = $(if ($Version.Contains("-")) {"docker-ci.axoom.cloud"} else {"docker.axoom.cloud"})
0install run http://assets.axoom.cloud/tools/ax.xml release --verbose --refresh asset.yml $Version --arg DOCKER_REGISTRY=$dockerRegistry

popd
