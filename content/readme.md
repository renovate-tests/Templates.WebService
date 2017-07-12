# Axoom.MyService

This repository contains the source code for Axoom.MyService.

Run `build.ps1` to compile the source code and then package the result as a Docker image.
This script takes a version number as an input argument. The source code itself contains no version numbers. Instead version numbers should be determined at build time using [GitVersion](http://gitversion.readthedocs.io/).

Run `docker-compose up` and then navigate to http://localhost:12345/swagger/ to test locally.

The directory `assets` contains a Docker Compose file for running the service in a production-like environment. It is usually combined with other Compose files, e.g., by a release management or provisioning system.
