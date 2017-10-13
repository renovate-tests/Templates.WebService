Param ([string]$Version = "0.1-dev")
$ErrorActionPreference = "Stop"
Push-Location $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

dotnet clean "Axoom.MyService.NoDocker.sln"
dotnet msbuild /t:Restore /t:Build /t:Publish /p:PublishDir=./obj/Docker/publish /p:Configuration=Release /p:Version=$Version "Axoom.MyService.NoDocker.sln"
dotnet test --configuration Release --no-build "Axoom.MyService.UnitTests/Axoom.MyService.UnitTests.csproj"

Pop-Location
