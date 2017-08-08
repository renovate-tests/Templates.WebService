# My Service

This repository contains the source code for My Service.

Run `build.ps1` to compile the source code and package the result in Docker images.
This script takes a version number as an input argument. The source code itself contains no version numbers. Instead version numbers should be determined at build time using [GitVersion](http://gitversion.readthedocs.io/).
The `release` directory contains Docker Compose templates for running in production environments. The build script injects version numbers into these templates.

For local testing:

    cd src
    ./build.ps1
    docker-compose up --build

 * My Service API: http://localhost:12345/swagger/
