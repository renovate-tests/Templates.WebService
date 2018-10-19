# My Service

This repository contains the source code for My Service.

## Development

Run `build.ps1` to compile the source code and package the result in Docker images. This script takes a version number as an input argument. The source code itself contains no version numbers. Instead version numbers should be determined at build time using [GitVersion](http://gitversion.readthedocs.io/).

Configuration overrides for local development are specified in:
- [launchSettings.json](src/Service/Properties/launchSettings.json) for IDEs
- [docker-compose.override.yml](src/docker-compose.override.yml) for Docker Compose

To build and run locally with Docker Compose:

    cd src
    ./build-dotnet.ps1
    docker-compose up --build

 * My Service API: http://localhost:12345/swagger/
