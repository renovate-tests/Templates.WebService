# My Service

This repository contains the source code for My Service.

Run `build.ps1` to compile the source code and package the result as a Docker image.
This script takes a version number as an input argument. The source code itself contains no version numbers. Instead version numbers should be determined at build time using [GitVersion](http://gitversion.readthedocs.io/).

The `assets` directory contains Docker Compose files for running in a production-like environment.
The build script creates copies with injected version numbers in the `artifacts` directory. These are usually combined with other Compose files by a provisioning system.

For local testing:

    cd src
	./build-dotnet.ps1
	docker-compose up -d
	open http://localhost:12345/swagger/
