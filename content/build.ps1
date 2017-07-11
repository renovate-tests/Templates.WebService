Param ([Parameter(Mandatory=$True)][string]$Version, [string]$DockerRegistry = "docker.axoom.cloud")
$ErrorActionPreference = "Stop"
$SolutionName = "Axoom.MyService"
$ImageName = "my_image"

pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)
pushd src

dotnet clean "$SolutionName.NoDocker.sln"
dotnet msbuild /t:Restore /t:Build /t:Publish /p:PublishDir=./obj/Docker/publish /p:Configuration=Release /p:Version=$Version "$SolutionName.NoDocker.sln"
dotnet test --configuration Release --no-build "$SolutionName.UnitTests/$SolutionName.UnitTests.csproj"

# Build Docker Image
docker build -t "${DockerRegistry}/${ImageName}:${Version}" $SolutionName

popd

# Inject version information into Docker Compose files
if (!(Test-Path artifacts)) { mkdir artifacts | Out-Null }
foreach ($x in (ls assets -Filter "*.yml"))
{
   (Get-Content $x.FullName -Encoding "UTF8").Replace('${DOCKER_REGISTRY}', $DockerRegistry).Replace('${VERSION}', $Version) | Out-File ("artifacts\" + $x.Name) -Encoding "UTF8"
}

popd
