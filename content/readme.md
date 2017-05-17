# MySolutionName

This repository contains the source code for the MySolutionName webservice.

Run `build.ps1` to compile the source code and then package the result as a Docker image.
This script takes a version number as an input argument. The source code itself contains no version numbers. Instead version numbers should be determined at build time using [GitVersion](gitversion.readthedocs.io).

Use `src/MySolutionName.Docker.sln` to debug the service from within Visual Studio while deployed inside a Docker container.
