Param ([string]$Version = "")
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

if ($Version -eq "")
{
  $DockerPrefix = ""
  $DockerSuffix = ""
}
else
{
  $DockerRegistry = if ($Version.Contains("-")) {"docker-ci.axoom.cloud"} else {"docker.axoom.cloud"}
  $DockerPrefix = "${DockerRegistry}/"
  $DockerSuffix = ":${Version}"
}

docker build -t "${DockerPrefix}my_service${DockerSuffix}" "Axoom.MyService"

popd
