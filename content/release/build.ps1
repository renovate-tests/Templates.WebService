Param ([Parameter(Mandatory=$True)][string]$Version)
$ErrorActionPreference = "Stop"
Push-Location $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

$DockerRegistry = $(if ($Version.Contains("-")) {"docker-ci.axoom.cloud"} else {"docker.axoom.cloud"})

# Docker Compose templates
foreach ($file in (Get-ChildItem -Filter *.yml.template))
{
  $template = Get-Content $file.FullName -Encoding "UTF8"
  $output = $template.Replace('${DOCKER_REGISTRY}', $DockerRegistry).Replace('${VERSION}', $Version)
  $outputFile = $file.Name.Replace(".yml.template", "-$Version.yml")
  echo "Writing $outputFile"
  $output | Out-File $outputFile -Encoding "UTF8"
}

Pop-Location
