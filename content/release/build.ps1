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

# Zero Install templates
$released = Get-Date -Format "yyyy-MM-dd"
$stability = if($Version.Contains("-")) {"developer"} else {"stable"}
foreach ($file in (Get-ChildItem -Filter *.xml.template))
{
   0install run http://0install.net/tools/0template.xml $file.FullName version=$Version released=$released stability=$stability
}

Pop-Location
