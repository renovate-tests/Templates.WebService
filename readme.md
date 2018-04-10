# AXOOM WebService .NET Core Template

This is the documentation for the AXOOM Webservice .NET Core project template---The way we recommend to build Apps to run on the AXOOM Platform.

## Purpose

We created this template to serve as both, a template to enable quick starts to create .NET Core Apps and also as a living documentation of how to work with the AXOOM Platform.

The AXOOM Platform demands a set of features of each and every service. This is the only way to ensure operability and maintainabilty from our side.
This template comes with a default implementation for all of these aspects:
- startup resilience
- monitoring metrics endpoints
- AXOOM logging standards configured
- Environment variable based configuration

The template is intentionally opinionated on architecture to increase cohesion/synergy between different players on the platform.
For now we are focusing on .NET to provide best development experience, but other technologies work fine too because all interop is done via standard web technologies.
In the future we will also provide Maven archetypes and such to provide the same benefit to users of different languages.

There are also Templates for
- [Libraries]()
- [Services]()
- [Portal Apps]()

## Getting Started

This is the quick guide how to setup a development environment for .NET Core Apps.

1. Make sure you have installed:
    - Vistual Studio 2017 / Jetbrains Rider
    - [.NET Core SDK & Runtime 2.0.0](https://download.microsoft.com/download/0/F/D/0FD852A4-7EA1-4E2A-983A-0484AC19B92C/dotnet-sdk-2.0.0-win-x64.exe)
    - Optional: Docker for Windows
2. Open the shell you are most familiar with
3. Run the following commands:
    ```
    dotnet new --install Axoom.Templates.Library::*
    dotnet new --install Axoom.Templates.Service::*
    dotnet new --install Axoom.Templates.WebService::*
    dotnet new --install Axoom.Templates.PortalApp::*
    ```
    This will install all AXOOM project templates on your local machine.
4. Run:
    ```
    mkdir MyOrg.MyService
    cd MyOrg.MyService
    ```
5. To finally create your new web service project run:
    ```
    dotnet new axoom-webservice --dockerName "my-service" --friendlyName "My Service" --description "my very detailed description"
    ```

## Project Structure

Let's have look at the project structure.
In the following, we will use `Axoom.MyService` as the default namespace, this is obviously different in your case.

### Top-level Files
The template comes with a set of default top-level files.
These are:

- `.gitattributes`: Set special git attributes to specific paths ([See the docs](https://git-scm.com/docs/gitattributes)). 
                    We use this to disable linebreak normalization.
- `.gitingore`: Ignore files while adding files into a git commit ([See the docs](https://git-scm.com/docs/gitignore)).
- `GitVersion.yml`: This is used by [GitVersion](https://github.com/GitTools/GitVersion) to extract version information from your git commits. 
                    We use this in our continous integration pipeline, to keep the code seperate from versioning. 

### Top-level Directories
The template comes with a set of default top-level directories.

| Directory   | Description                                               |
| ----------- | --------------------------------------------------------- |
| `src`       | Source Code, test and build scripts, docker-compose files |
| `release`   | `AXOOM Provisioning` release files                        |
| `deploy`    | `AXOOM Provisioning` deployment files                     |
| `artifacts` | Compile output                                            |

### Build Scripts

### `*.NoDocker.sln`

### The Projects

Since, in most cases, a webservice is not only used by a graphical frontend, we usually also build clients to consume the exposed service API.
This results in the following project structure:

- `Axoom.MyService`: The actual web service project.
- `Axoom.MyService.Client`: The service client project.
- `Axoom.MyService.Dto`: The dtos shared between webservice and its client.
- `Axoom.MyService.UnitTests`: The tests for the service and the client as well. This includes in-memory API contract tests.
                               __Note:__ All projects share the same namespace.

### General Design Considerations

#### Formats, Libraries, ...

To create a certain level of consistency across all our projects, we created a set of conventions, which lead to an easy to manage CI/CD pipline.

##### GitVersion
As mentioned earlier, we decided to use `GitVersion` to extract version information from our git commits. 
Hence, we make use of git tags to create a release whereas all other commits are pre-releases by default.
The `GitVersion.yml` file configures GitVersion to extract a Version number like the following from an untagged commit:
```
0.1.1-pre0045-20180404094115
```
| Version Part     | Description                                                                                               |
| ---------------- | --------------------------------------------------------------------------------------------------------- |
| `0.1.1`          | The latest tag with a bumped patch version                                                                |
| `pre0045`        | Indicates, that it's a prerelease (of `0.1.1`) and 45 commits have been made since the last tag (`0.1.0`) |
| `20180404094115` | The Timestamp _04/04/2018 09:41:15_                                                                       |

Whereas `0.1.1` is extracted from a commit with tag `0.1.1`.

##### OneFlow


##### SemVer

To make a CI/CD pipeline successful it's extremely relevant, to get the most information out of your versioning.
Using [SemVer](https://semver.org/), you create the level of predictability of upgrade impact you need to enable continous delivery.
Furthermore it enables looser coupling between diverse assets if you can make sure, a certain update will not break the API consumed by other services.

So in very little words, a semantic version consists of three parts which have to be bumped on certain changes:
- _Major_: Breaking changes
- _Minor_: Features added
- _Patch_: Bugs have been fixed

__Special versions:__
- `1.0.0`: Your asset is mature enough to be production ready
- `0.1.0`: The initial version of each asset

There is a exception in how to handle versions before first production readyness, where we decided to use __Minor for breaking changes__ and __Patch for feature additions and bug fixes as well__, since in the ongoing development phase you are not bug aware anyway.

##### YAML Config Files

We decided to use YAML files for configuration purposes.
JSON is for sure a great format for serialization and is as well human-readable, but YAML is furthermore human-writable and supports comments, what you actually want from a configuration format.

##### TypedRest

[TypedRest](https://github.com/TypedRest/TypedRest-DotNet) is a library supporting you on building high-level REST Clients.
Using it feels like implementing the [Repository Pattern](https://msdn.microsoft.com/en-us/library/ff649690.aspx) instead of handling urls.
Furthermore TypedRest relies on standard HTTP technologies, and simplifies use cases like paging and so on.
Using the library guides to a clean and consistent API design.

#### Layers and Slices
We for ourselves found, it's a good practice to architect services into slices instead of only thinking in layers.
In times of _micro services_ services tend to become more and more stateless and isolated. 
This increases availabilty, operability and simplifies technical ownership.
In the process of architecting and implementing, we learn a lot about the problem domain we are working in. In particular, we get to know the diverse actors in domain processes, the necessary/desired cardinalities and such.
So, we mostly don't start building a micro service but a monolith---simply because we don't know what we actually need.
Thinking in slices makes it extremely easy to reign our monolith's (internal) dependencies in an isolated way, so that we can extract projects or even a whole new service, implementing a single slice of our domain.

__So what is a slice?__ Actually it's a vertical cut through your application representing a single part of the domain or an use case.
Having a vertical cut does not mean you have to drop your thoughts about layers, if your dependencies tend to get more and more complex, layers will still improve the design of your slice. The actual difference is then that you don't think of __the__ persistence layer anymore but about a _customers persistence layer_ and a _products persistence layer_.

The template also sticks to slices. Therefore you find all the sub directories named by a part of the domain (`Contacts` in the included example).

__Further reading:__
- https://www.thoughtworks.com/insights/blog/slicing-your-development-work-multi-layer-cake
- http://deviq.com/vertical-slices/


## Contributing

To contribute create pull requests in [this](https://github.com/) Git repository.

The repository contains a `content` directory. This is what actually gets packaged into the template.
The file `content/template.config/template.json` controls which placeholders are replaced during instantiation.

## Building

Run `build.ps1` to package the template as a NuGet package.
This script takes a version number as an input argument. The source code itself contains no version numbers. Instead version numbers should be determined at build time using [GitVersion](http://gitversion.readthedocs.io/).

The `content` directory is what actually gets packaged into the template. The file `content/template.config/template.json` controls which placeholders are replaced during instantiation.